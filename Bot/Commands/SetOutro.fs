module Bot.Commands.SetOutro

open Discord
open Discord.WebSocket
open Bot.Music.Download
open Bot.Wrapper.SlashCommandBuilder
open Bot.Wrapper.SlashCommandOptionBuilder

let setOutroCommandHandler (url: string) (client: IDiscordClient) (command: SocketSlashCommand) = task {
    do! command.RespondAsync("downloading")
    do! download url $"outro-{command.User.Id}.%%(ext)s"
    do! command.ModifyOriginalResponseAsync(fun message -> message.Content <- "done")
        |> Async.AwaitTask |> Async.Ignore
}
    
let setOutroCommand =
    newSlashCommand()
    |> withCommandName "set_outro"
    |> withCommandDescription "set a track to play on leave"
    |> withCommandOption (
        newStringOption()
        |> withOptionName "url"
        |> withOptionDescription "youtube url" )
    |> withHandler setOutroCommandHandler