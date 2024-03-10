module Bot.Events.Ready

open System.Threading.Tasks
open Discord
open Bot.Wrapper.SlashCommandBuilder

let ready (client: IDiscordClient) (): Task = task {
        let! guild = client.GetGuildAsync(543719732100988943UL)
        
        let builder =
            newSlashCommand
            |> withName "cool"
            |> withDescription "cool command"
            
        do! guild.CreateApplicationCommandAsync(builder.Build()) |> Async.AwaitTask |> Async.Ignore
        
        ()
    }