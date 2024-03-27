namespace Bot

open System
open System.Threading.Tasks
open Discord
open Discord.WebSocket
open Bot.Commands.DownloadCommands
open Bot.Events.Ready
open Bot.Events.SlashCommandExecuted
open Bot.Events.UserVoiceStateUpdated
open Bot.Queue

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
        
        let commands = [setIntroCommand; setOutroCommand]
        
        let queue = newLockedQueue<string>()
        
        client.add_Ready (ready client commands)
        client.add_SlashCommandExecuted (slashCommandExecuted client commands)
        client.add_UserVoiceStateUpdated (userVoiceStateUpdated queue)
        
        do! Task.Delay(-1)
    }
