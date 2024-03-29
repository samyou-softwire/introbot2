module Bot.Commands.DownloadCommands

open Discord
open Discord.WebSocket
open Bot.Music.YtDlp
open Bot.Wrapper.SlashCommandBuilder
open Bot.Wrapper.SlashCommandOptionBuilder

let downloadCommandHandler (prefix: string) (_end: string) (start: string) (url: string) (client: IDiscordClient) (command: SocketSlashCommand) = task {
    let range = $"{start}-{_end}"
    do! command.RespondAsync($"downloading <{url}> between {range}")
    let! success = download url range $"{prefix}-{command.User.Id}.%%(ext)s"
    do! command.ModifyOriginalResponseAsync(fun message -> message.Content <- if success then $"done <{url}> between {range}" else "error")
        |> Async.AwaitTask |> Async.Ignore
}
    
let setIntroCommand =
    newSlashCommand()
    |> withCommandName "set_intro"
    |> withCommandDescription "set a track to play on join"
    |> withCommandOption (
        newStringOption()
        |> withOptionName "url"
        |> withOptionDescription "youtube url" )
    |> withCommandOption (
        newStringOption()
        |> withOptionName "start"
        |> withOptionDescription "xx:xx")
    |> withCommandOption (
        newStringOption()
        |> withOptionName "end"
        |> withOptionDescription "xx:xx")
    |> withHandler (downloadCommandHandler "intro")

let setOutroCommand =
    newSlashCommand()
    |> withCommandName "set_outro"
    |> withCommandDescription "set a track to play on leave"
    |> withCommandOption (
        newStringOption()
        |> withOptionName "url"
        |> withOptionDescription "youtube url" )
    |> withCommandOption (
        newStringOption()
        |> withOptionName "start"
        |> withOptionDescription "xx:xx")
    |> withCommandOption (
        newStringOption()
        |> withOptionName "end"
        |> withOptionDescription "xx:xx")
    |> withHandler (downloadCommandHandler "outro")