module Bot.Events.SlashCommandExecuted

open System.Threading.Tasks
open Discord.WebSocket

let slashCommandExecuted _ (command: SocketSlashCommand): Task = task {
    do! command.RespondAsync("you already know")
    
    ()
}