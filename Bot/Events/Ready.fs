module Bot.Events.Ready

open System.Threading.Tasks
open Discord
open Bot.Wrapper.SlashCommandBuilder

let ready (client: IDiscordClient) (commands: BuiltCommand<obj> list) (): Task = task {
    let! guilds = client.GetGuildsAsync()
    
    let commandProperties =
        commands
        |> List.map (fun command -> command.properties :> ApplicationCommandProperties)
        |> List.toArray
    
    for guild in guilds do
        do! guild.BulkOverwriteApplicationCommandsAsync(commandProperties) |> Async.AwaitTask |> Async.Ignore
    
    ()
}