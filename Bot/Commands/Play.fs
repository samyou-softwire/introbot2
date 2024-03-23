module Bot.Commands.Play

open Discord
open Discord.WebSocket
open Bot.Music.Download
open Bot.Wrapper.SlashCommandBuilder
open Bot.Wrapper.SlashCommandOptionBuilder

let playCommandHandler (url: string) (client: IDiscordClient) (command: SocketSlashCommand) =
    download url $"{command.User.Id}.%%(ext)s"
    "download complete"
    
let playCommand =
    newSlashCommand()
    |> withCommandName "play"
    |> withCommandDescription "test play a track"
    |> withCommandOption (
        newStringOption()
        |> withOptionName "urlly"
        |> withOptionDescription "youtube url" )
    |> withHandler playCommandHandler