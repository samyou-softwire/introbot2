module Bot.Events.ManageQueue

open System.Threading.Tasks
open Bot.Events.UserVoiceStateUpdated
open Bot.Queue
open Bot.Wrapper.Ffmpeg
open Discord.WebSocket

let joinChannel (channel: SocketVoiceChannel) = task {
    return! channel.ConnectAsync()
}

let rec queueManager (queue: LockedQueue<QueueItem>) = task {
    let mutable oldChannel: SocketVoiceChannel = null
    let mutable connection = null
    while true do
        if getQueueLength queue > 0 then
            let queueItem = popFromQueue queue
            
            // if not in a channel or needs to move
            if oldChannel = null || queueItem.channel.Id <> oldChannel.Id then
                // establish new connection
                let! newConnection = joinChannel queueItem.channel
                connection <- newConnection
                ()
            
            do! playIntoChannel connection queueItem.path queueItem.channel |> Async.AwaitTask
            
            oldChannel <- queueItem.channel
            
            // if queue is now empty
            if getQueueLength queue = 0 then
                do! Task.Delay(5000)
                // leave
                do! connection.StopAsync()
                connection <- null
                oldChannel <- null
            
        do! Task.Delay(100)
}