module Bot.Wrapper.SlashCommandBuilder

open Discord
open Bot.Wrapper.SlashCommandOptionBuilder
open Bot.Reflection
open Discord.WebSocket

type CommandBuilder<'a> = {
    innerBuilder: SlashCommandBuilder
    arguments: CommandOptionType list
}

type BuiltCommand<'a> = {
    properties: SlashCommandProperties
    handler: IDiscordClient -> SocketSlashCommand -> string
}

type CommandHandler = BuiltCommand<obj>

let newSlashCommand: unit -> CommandBuilder<IDiscordClient -> SocketSlashCommand -> string> = fun _ -> {
    innerBuilder = SlashCommandBuilder()
    arguments = [] 
}

let withCommandName<'a> (name: string) (builder: CommandBuilder<'a>): CommandBuilder<'a> = { 
    builder with innerBuilder = builder.innerBuilder.WithName(name) 
}

let withCommandDescription<'a> (name: string) (builder: CommandBuilder<'a>): CommandBuilder<'a> = {
    builder with innerBuilder = builder.innerBuilder.WithDescription(name) 
}
    
let withCommandOption<'a, 'b> (optionBuilder: CommandOptionBuilder<'b>) (builder: CommandBuilder<'a>): CommandBuilder<'b -> 'a> = {
    innerBuilder = builder.innerBuilder.AddOption(optionBuilder.innerBuilder)
    arguments = optionBuilder._type :: builder.arguments
}

let withHandler<'a> (handler: 'a -> IDiscordClient -> SocketSlashCommand -> string) (builder: CommandBuilder<'a -> IDiscordClient -> SocketSlashCommand -> string>) = {
    properties = builder.innerBuilder.Build()
    handler = fun (client: IDiscordClient) (command: SocketSlashCommand) ->
        let options = List.ofSeq command.Data.Options
        let handled = (doHandle options handler)
        (handled client command) :?> string
}
