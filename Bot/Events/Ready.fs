module Bot.Events.Ready

open System.Diagnostics
open System.Threading.Tasks
open Discord
open Bot.Wrapper.SlashCommandBuilder
open Discord.Audio

let ready (client: IDiscordClient) (commands: BuiltCommand<obj> list) (): Task = task {
    let! guild = client.GetGuildAsync(543719732100988943UL)
    
    let commandProperties =
        commands
        |> List.map (fun command -> command.properties :> ApplicationCommandProperties)
        |> List.toArray
    
    do! guild.BulkOverwriteApplicationCommandsAsync(commandProperties) |> Async.AwaitTask |> Async.Ignore
    
    ()
}