namespace Bot

open System.IO
open System.Threading.Tasks
open Discord
open Discord.WebSocket
open FSharp.Data.JsonProvider

module Bot =
    type Secrets = JsonProvider<"""../secrets.json""">
    
    let start = task {
        let log (message: LogMessage): Task =
            printfn $"%s{message.ToString()}"
            Task.CompletedTask
            
        let secrets = Secrets.Parse(File.ReadLines("../../../../secrets.json") |> String.concat "")
            
        let client = new DiscordSocketClient()
        client.add_Log log
        
        do! client.LoginAsync(TokenType.Bot, secrets.ApiKey)
        do! client.StartAsync()
        
        do! Task.Delay(-1)
    }
