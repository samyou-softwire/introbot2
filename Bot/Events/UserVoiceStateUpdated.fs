module Bot.Events.UserVoiceStateUpdated

open System.Threading.Tasks
open Bot.Queue
open Discord
open Discord.WebSocket

type PlayerQueue = LockedQueue<string>

let joinAndPlay (path: string) (channel: SocketVoiceChannel) = task {
    let! client = channel.ConnectAsync()
    
    ()
}

let playTheme (theme: string) (user: SocketUser) (channel: SocketVoiceChannel) = task {
    do! joinAndPlay $"./theme-cache/{theme}-{user.Id}.opus" channel
    ()
}

let playIntro = playTheme "intro"

let playOutro = playTheme "outro"

let userVoiceStateUpdated (playerQueue: PlayerQueue) (user: SocketUser) (oldState: SocketVoiceState) (newState: SocketVoiceState): Task = task {
    if user.IsBot then return ()
    elif newState.VoiceChannel <> null then
        do! playIntro user newState.VoiceChannel
        return ()
    elif oldState.VoiceChannel <> null then
        do! playOutro user oldState.VoiceChannel
        return ()
    else return ()
}