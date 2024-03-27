module Bot.Events.UserVoiceStateUpdated

open System.Threading.Tasks
open Bot.Queue
open Discord.WebSocket

type QueueItem = {
    path: string;
    channel: SocketVoiceChannel;
}

type PlayerQueue = LockedQueue<QueueItem>

let joinAndPlay (path: string) (queue: PlayerQueue) (channel: SocketVoiceChannel) = task {
    pushToQueue queue {
        path = path
        channel = channel 
    }
}

let playTheme (theme: string) (queue: PlayerQueue) (user: SocketUser) (channel: SocketVoiceChannel) = task {
    do! joinAndPlay $"./theme-cache/{theme}-{user.Id}.opus" queue channel
}

let playIntro = playTheme "intro"

let playOutro = playTheme "outro"

let userVoiceStateUpdated (queue: PlayerQueue) (user: SocketUser) (oldState: SocketVoiceState) (newState: SocketVoiceState): Task = task {
    if user.IsBot then return ()
    elif newState.VoiceChannel <> null then
        do! playIntro queue user newState.VoiceChannel
    elif oldState.VoiceChannel <> null then
        do! playOutro queue user oldState.VoiceChannel
    else ()
}