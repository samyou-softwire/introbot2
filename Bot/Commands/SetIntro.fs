module Bot.Commands.SetIntro

open Discord
open Discord.WebSocket
open Bot.Music.Download
open Bot.Wrapper.SlashCommandBuilder
open Bot.Wrapper.SlashCommandOptionBuilder

let setIntroCommandHandler (url: string) (client: IDiscordClient) (command: SocketSlashCommand) = task {
    do! command.RespondAsync("downloading")
    do! download url $"intro-{command.User.Id}.%%(ext)s"
    do! command.ModifyOriginalResponseAsync(fun message -> message.Content <- "done")
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
    |> withHandler setIntroCommandHandler