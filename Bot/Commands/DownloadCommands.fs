module Bot.Commands.DownloadCommands

open Discord
open Discord.WebSocket
open Bot.Music.YtDlp
open Bot.Wrapper.SlashCommandBuilder
open Bot.Wrapper.SlashCommandOptionBuilder

let downloadCommandHandler (prefix: string) (_end: string) (start: string) (url: string) (client: IDiscordClient) (command: SocketSlashCommand) = task {
    do! command.RespondAsync("downloading")
    
    let start = if start <> null then start else "00:00"
    let _end = if _end <> null then _end else "inf"
    let range = $"{start}-{_end}"
    
    let! success = download url range $"{prefix}-{command.User.Id}.%%(ext)s"
    do! command.ModifyOriginalResponseAsync(fun message -> message.Content <- if success then "done" else "error")
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
        |> withOptionDescription "xx:xx" 
        |> withOptionOptional )
    |> withCommandOption (
        newStringOption()
        |> withOptionName "end"
        |> withOptionDescription "xx:xx" 
        |> withOptionOptional )
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
        |> withOptionDescription "xx:xx" 
        |> withOptionOptional )
    |> withCommandOption (
        newStringOption()
        |> withOptionName "end"
        |> withOptionDescription "xx:xx" 
        |> withOptionOptional )
    |> withHandler (downloadCommandHandler "outro")