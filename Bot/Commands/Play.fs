module Bot.Commands.Play

open System.Threading.Tasks
open Discord
open Discord.WebSocket
open Bot.Music.Download
open Bot.Wrapper.SlashCommandBuilder
open Bot.Wrapper.SlashCommandOptionBuilder

let playCommandHandler (url: string) (client: IDiscordClient) (command: SocketSlashCommand) = task {
    do! command.RespondAsync("downloading")
    do! download url $"{command.User.Id}.%%(ext)s"
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