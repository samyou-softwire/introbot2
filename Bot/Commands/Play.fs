module Bot.Commands.Play

open System.Threading.Tasks
open Discord
open Discord.WebSocket
open Bot.Music.Download
open Bot.Wrapper.SlashCommandBuilder
open Bot.Wrapper.SlashCommandOptionBuilder

let playCommandHandler (url: string) (client: IDiscordClient) (command: SocketSlashCommand) = task {
    let onStart = task {
        do! command.DeferAsync()
        printf "onstart2!"
    }
    
    let onEnd = task {
        do! command.FollowupAsync("done!") |> Async.AwaitTask |> Async.Ignore
        printf "onend2!"
    }
    
    do! download url $"{command.User.Id}.%%(ext)s" onStart onEnd
    return "download complete"
}
    
let playCommand =
    newSlashCommand()
    |> withCommandName "play"
    |> withCommandDescription "test play a track"
    |> withCommandOption (
        newStringOption()
        |> withOptionName "url"
        |> withOptionDescription "youtube url" )
    |> withHandler playCommandHandler