module Bot.Wrapper.SlashCommandOptionBuilder

open Discord

let newSlashCommandOption = SlashCommandOptionBuilder()

let withOptionName (name: string) (builder: SlashCommandOptionBuilder): SlashCommandOptionBuilder =
    builder.WithName(name)

let withOptionDescription (description: string) (builder: SlashCommandOptionBuilder): SlashCommandOptionBuilder =
    builder.WithDescription(description)

let withOptionType (type_: ApplicationCommandOptionType) (builder: SlashCommandOptionBuilder): SlashCommandOptionBuilder =
    builder.WithType(type_)

let withOptionRequired (isRequired: bool) (builder: SlashCommandOptionBuilder): SlashCommandOptionBuilder =
    builder.WithRequired(isRequired)
