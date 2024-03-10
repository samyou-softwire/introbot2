namespace Bot

open System
open System.IO
open System.Threading.Tasks
open Discord
open Discord.WebSocket
open FSharp.Data.JsonProvider
open Wrapper.SlashCommandBuilder

module Bot =
    let ready (client: IDiscordClient) (): Task = task {
        let! guild = client.GetGuildAsync(543719732100988943UL)
        
        let builder =
            newSlashCommand
            |> withName "cool"
            |> withDescription "cool command"
            
        do! guild.CreateApplicationCommandAsync(builder.Build()) |> Async.AwaitTask |> Async.Ignore
        
        ()
    }
    
    let start = task {
        let log (message: LogMessage): Task =
            printfn $"%s{message.ToString()}"
            Task.CompletedTask
            
        let client = new DiscordSocketClient()
        client.add_Log log
        
        let apiKey = System.Environment.GetEnvironmentVariable("API_KEY")
        
        do! client.LoginAsync(TokenType.Bot, apiKey)
        do! client.StartAsync()
        
        client.add_Ready (ready client)
        
        do! Task.Delay(-1)
    }
