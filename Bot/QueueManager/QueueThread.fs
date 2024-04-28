module Bot.QueueManager.QueueThread

open System.Threading.Tasks
open Bot.QueueManager.Queue
open Bot.Wrapper.Ffmpeg
open Discord.Audio
open Discord.WebSocket

type MusicQueueState =
    | Started
    | Existing of IAudioClient * SocketVoiceChannel
    
type MusicQueueAction =
    | Join of SocketVoiceChannel
    | JustPlay of IAudioClient
    
type MusicQueueOngoingState = {
    connection: IAudioClient
    loopsRemaining: int
}
    
let handleItemInQueue (queueItem: QueueItem) (state: MusicQueueState) = task {
    let action =
        match state with
        | Started -> Join queueItem.channel
        | Existing (connection, channel) ->
            if channel.Id = queueItem.channel.Id then
                JustPlay connection
            else
                Join queueItem.channel
    
    let! connection =
        match action with
        | Join channel -> channel.ConnectAsync()
        | JustPlay connection -> task { return connection }
        
    do! playIntoChannel connection queueItem.path queueItem.channel |> Async.AwaitTask
    
    return Existing (connection, queueItem.channel)
}

let rec handleItemsInQueue (queue: LockedQueue<QueueItem>) (state: MusicQueueState) = async {
    if getQueueLength queue > 0 then
        let queueItem = popFromQueue queue

        let! nextState = handleItemInQueue queueItem state |> Async.AwaitTask

        return! handleItemsInQueue queue nextState
    else
        return
            match state with
            | Started -> failwith "connection wasn't started!"
            | Existing (connection, _) -> connection
}

let disposeIfNecessary (musicQueueOngoingState: MusicQueueOngoingState option) = task {
    match musicQueueOngoingState with
    | Some musicQueueOngoingState ->
        if musicQueueOngoingState.loopsRemaining = 0 then
            do! musicQueueOngoingState.connection.StopAsync()
            return None
        else
            return Some {
                musicQueueOngoingState
                with loopsRemaining = musicQueueOngoingState.loopsRemaining - 1;
            }
    | None -> return None
}

let watchQueue (queue: LockedQueue<QueueItem>) = task {
    let mutable musicQueueOngoingState: MusicQueueOngoingState option = None
    
    while true do
        if getQueueLength queue > 0 then
            let! connection = handleItemsInQueue queue Started
            musicQueueOngoingState <-
                Some {
                    connection = connection
                    loopsRemaining = 50 // 50 * 100ms = 5 seconds
                }
        
        let! newMusicQueueOngoingState = disposeIfNecessary musicQueueOngoingState
        musicQueueOngoingState <- newMusicQueueOngoingState
            
        do! Task.Delay(100)
}