module Bot.Wrapper.SlashCommandOptionBuilder

open Discord

type CommandOptionBuilder<'a> = {
    innerBuilder: SlashCommandOptionBuilder
}

let newStringOption: CommandOptionBuilder<string> = {
    innerBuilder = SlashCommandOptionBuilder().WithType(ApplicationCommandOptionType.String) 
} 

let newIntegerOption: CommandOptionBuilder<int> = {
    innerBuilder = SlashCommandOptionBuilder().WithType(ApplicationCommandOptionType.Integer) 
}

let withOptionName<'a> (name: string) (builder: CommandOptionBuilder<'a>): CommandOptionBuilder<'a> = {
    innerBuilder = builder.innerBuilder.WithName(name) 
}

let withOptionDescription<'a> (description: string) (builder: CommandOptionBuilder<'a>): CommandOptionBuilder<'a> = {
    innerBuilder = builder.innerBuilder.WithDescription(description) 
}

let withOptionRequired<'a> (isRequired: bool) (builder: CommandOptionBuilder<'a>): CommandOptionBuilder<'a> = {
    innerBuilder = builder.innerBuilder.WithRequired(isRequired)
}
