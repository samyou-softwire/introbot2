module Bot.Events.UserVoiceStateUpdated

open System.IO
open System.Threading.Tasks
open Bot.Queue
open Discord.WebSocket

type QueueItem = {
    path: string;
    channel: SocketVoiceChannel;
}

type PlayerQueue = LockedQueue<QueueItem>

let addToQueue (path: string) (queue: PlayerQueue) (channel: SocketVoiceChannel) = task {
    if File.Exists(path) then
        pushToQueue queue {
            path = path
            channel = channel 
        }
}

let playTheme (theme: string) (queue: PlayerQueue) (user: SocketUser) (channel: SocketVoiceChannel) = task {
    do! addToQueue $"./theme-cache/{theme}-{user.Id}.opus" queue channel
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