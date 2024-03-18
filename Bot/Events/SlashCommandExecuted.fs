module Bot.Events.SlashCommandExecuted

open System.Threading.Tasks
open Discord
open Discord.WebSocket
open Bot.Wrapper.SlashCommandBuilder

let slashCommandExecuted (client: IDiscordClient) (commands: CommandHandler list) (command: SocketSlashCommand): Task = task {
    let hasName (handler: CommandHandler) =
        handler.properties.Name.GetValueOrDefault() = command.CommandName
        
    let commandHandler = List.find hasName commands
    
    let response = commandHandler.handler client command
        
    do! command.RespondAsync(response)
    
    ()
}