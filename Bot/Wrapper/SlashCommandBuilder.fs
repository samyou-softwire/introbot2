module Bot.Wrapper.SlashCommandBuilder

open Discord
open Bot.Wrapper.SlashCommandOptionBuilder

type CommandBuilder<'a> = {
    innerBuilder: SlashCommandBuilder
}

type BuiltCommand<'a> = {
    innerBuilder: SlashCommandBuilder
    handler: 'a
}

let newSlashCommand: CommandBuilder<IDiscordClient -> string> = {
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

let withHandler<'a> (handler: 'a) (builder: CommandBuilder<'a>) = {
    innerBuilder = builder.innerBuilder
    handler = handler
}
