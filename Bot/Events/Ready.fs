module Bot.Events.Ready

open System.Threading.Tasks
open Discord
open Bot.Wrapper.SlashCommandBuilder

let ready (client: IDiscordClient) (commands: BuiltCommand<obj> list) (): Task = task {
    let! guild = client.GetGuildAsync(543719732100988943UL)
    
    for command in commands do
        do! guild.CreateApplicationCommandAsync(command.properties) |> Async.AwaitTask |> Async.Ignore
    
    ()
}