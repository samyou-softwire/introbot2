namespace Bot

open System
open System.Threading.Tasks
open Discord
open Discord.WebSocket
open Bot.Commands.Cool
open Bot.Commands.Play
open Bot.Events.Ready
open Bot.Events.SlashCommandExecuted

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
        
        let commands = [coolCommand; playCommand]
        
        client.add_Ready (ready client commands)
        client.add_SlashCommandExecuted (slashCommandExecuted client commands)
        
        do! Task.Delay(-1)
    }
