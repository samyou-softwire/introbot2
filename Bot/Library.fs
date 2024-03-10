namespace Bot

open System
open System.Threading.Tasks
open Discord
open Discord.WebSocket
open Bot.Events.Ready

module Bot =
    let start = task {
        let log (message: LogMessage): Task =
            printfn $"%s{message.ToString()}"
            Task.CompletedTask
            
        let client = new DiscordSocketClient()
        client.add_Log log
        
        let apiKey = Environment.GetEnvironmentVariable("API_KEY")
        
        do! client.LoginAsync(TokenType.Bot, apiKey)
        do! client.StartAsync()
        
        client.add_Ready (ready client)
        
        do! Task.Delay(-1)
    }
