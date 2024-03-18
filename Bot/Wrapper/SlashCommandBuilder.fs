module Bot.Wrapper.SlashCommandBuilder

open Discord
open Bot.Wrapper.SlashCommandOptionBuilder
open Discord.WebSocket

type CommandBuilder<'a> = {
    innerBuilder: SlashCommandBuilder
}

type BuiltCommand<'a> = {
    properties: SlashCommandProperties
    handler: IDiscordClient -> SocketSlashCommand -> string
}

type CommandHandler = BuiltCommand<obj>

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
        let head = options.Head
        // TODO: this can probably be lots more functional
        match box f with
        | :? (string -> obj) ->
            if (head.Type = ApplicationCommandOptionType.String) then doHandle options (f head.Value)
            else failwith("incorrect arg")
        | :? (int -> obj) ->
            if (head.Type = ApplicationCommandOptionType.Integer) then doHandle options (f head.Value)
            else failwith("incorrect arg")
        | _ -> failwith("unknown arg type")
            
        
    | _ -> failwith("incorrect handler has been bound")

let withHandler<'a> (handler: 'a -> IDiscordClient -> SocketSlashCommand -> string) (builder: CommandBuilder<'a -> IDiscordClient -> SocketSlashCommand -> string>) = {
    properties = builder.innerBuilder.Build()
    handler = fun (client: IDiscordClient) (command: SocketSlashCommand) ->
        let options = List.ofSeq command.Data.Options
        let handled = doHandle options handler
        handled client command
}
