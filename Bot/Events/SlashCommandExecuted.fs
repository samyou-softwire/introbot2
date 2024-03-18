module Bot.Events.SlashCommandExecuted

open System.Threading.Tasks
open Discord
open Discord.WebSocket
open Bot.Wrapper.SlashCommandBuilder

let slashCommandExecuted (client: IDiscordClient) (commands: BuiltCommand<obj> list) (command: SocketSlashCommand): Task = task {
    let folder (acc: string option) (cur: BuiltCommand<obj>) =
        match acc with
        | Some x -> Some x
        | None -> cur.handler client command
    
    let result = List.fold folder None commands
    
    let response =
        match result with
        | Some response -> response
        | None -> "Command not found!"
        
    do! command.RespondAsync(response)
    
    ()
}