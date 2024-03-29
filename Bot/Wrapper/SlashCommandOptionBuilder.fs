module Bot.Wrapper.SlashCommandOptionBuilder

open Discord

type CommandOptionType =
    | String
    | Integer

type CommandOptionBuilder<'a> = {
    innerBuilder: SlashCommandOptionBuilder
    _type: CommandOptionType
}

let newStringOption: unit -> CommandOptionBuilder<string> = fun _ -> {
    innerBuilder = SlashCommandOptionBuilder().WithType(ApplicationCommandOptionType.String).WithRequired(true)
    _type = String 
} 

let newIntegerOption: CommandOptionBuilder<int> = {
    innerBuilder = SlashCommandOptionBuilder().WithType(ApplicationCommandOptionType.Integer)
    _type = Integer
}

let withOptionName<'a> (name: string) (builder: CommandOptionBuilder<'a>): CommandOptionBuilder<'a> = {
    builder with innerBuilder = builder.innerBuilder.WithName(name) 
}

let withOptionDescription<'a> (description: string) (builder: CommandOptionBuilder<'a>): CommandOptionBuilder<'a> = {
    builder with innerBuilder = builder.innerBuilder.WithDescription(description) 
}

let withOptionRequired<'a> (isRequired: bool) (builder: CommandOptionBuilder<'a>): CommandOptionBuilder<'a> = {
    builder with innerBuilder = builder.innerBuilder.WithRequired(isRequired)
}
