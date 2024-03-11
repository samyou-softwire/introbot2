module Bot.Events.Ready

open System.Threading.Tasks
open Discord
open Bot.Wrapper.SlashCommandBuilder
open Bot.Wrapper.SlashCommandOptionBuilder

let ready (client: IDiscordClient) (): Task = task {
    let! guild = client.GetGuildAsync(543719732100988943UL)
    
    let builder =
        newSlashCommand
        |> withCommandName "cool"
        |> withCommandDescription "cool command"
        |> withCommandOption (newSlashCommandOption
            |> withOptionName "e"
            |> withOptionDescription "thingy"
            |> withOptionType ApplicationCommandOptionType.String
            |> withOptionRequired true)
        
    do! guild.CreateApplicationCommandAsync(builder.Build()) |> Async.AwaitTask |> Async.Ignore
    
    ()
}