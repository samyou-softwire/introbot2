module Bot.Wrapper.SlashCommandBuilder

open Discord
open Bot.Wrapper.SlashCommandOptionBuilder
open Discord.WebSocket

type CommandBuilder<'a> = {
    innerBuilder: SlashCommandBuilder
}

type BuiltCommand<'a> = {
    command: SlashCommandProperties
    handler: IDiscordClient -> SocketSlashCommand -> string
}

let newSlashCommand: CommandBuilder<IDiscordClient -> SocketSlashCommand -> string> = {
    innerBuilder = SlashCommandBuilder() 
}

let withCommandName<'a> (name: string) (builder: CommandBuilder<'a>): CommandBuilder<'a> = {
    innerBuilder = builder.innerBuilder.WithName(name) 
}

let withCommandDescription<'a> (name: string) (builder: CommandBuilder<'a>): CommandBuilder<'a> = {
    innerBuilder = builder.innerBuilder.WithDescription(name) 
}
    
let withCommandOption<'a, 'b> (optionBuilder: CommandOptionBuilder<'b>) (builder: CommandBuilder<'a>): CommandBuilder<'b -> 'a> = {
    innerBuilder = builder.innerBuilder.AddOption(optionBuilder.innerBuilder) 
}

let rec doHandle (options: SocketSlashCommandDataOption list) (handler: obj) =
    match box handler with
    | :? (IDiscordClient -> SocketSlashCommand -> string) as f -> f
    | :? (obj -> IDiscordClient -> SocketSlashCommand -> string) as f ->
        let arg = options.Head.Value
        doHandle options (f arg)
    | _ -> failwith("incorrect handler has been bound")

let withHandler<'a> (handler: 'a -> IDiscordClient -> SocketSlashCommand -> string) (builder: CommandBuilder<'a -> IDiscordClient -> SocketSlashCommand -> string>) = {
    command = builder.innerBuilder.Build()
    handler = fun (client: IDiscordClient) (command: SocketSlashCommand) ->
        let options = List.ofSeq command.Data.Options
        let handled: IDiscordClient -> SocketSlashCommand -> string = doHandle options handler
        handled client command
}