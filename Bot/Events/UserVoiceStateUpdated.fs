module Bot.Events.UserVoiceStateUpdated

open System.Threading.Tasks
open Discord
open Discord.WebSocket

let playIntro () = task {
    printfn "playing intro"
    ()
}

let playOutro () = task {
    printfn "playing outro"
    ()
}

let userVoiceStateUpdated (user: SocketUser) (oldState: SocketVoiceState) (newState: SocketVoiceState): Task = task {
    if user.IsBot then return ()
    elif newState.VoiceChannel <> null then
        do! playIntro()
        return ()
    elif oldState.VoiceChannel <> null then
        do! playOutro()
        return ()
    else return ()
}