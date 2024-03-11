module Bot.Wrapper.SlashCommandBuilder

open Discord

let newSlashCommand = SlashCommandBuilder()

let withCommandName (name: string) (builder: SlashCommandBuilder): SlashCommandBuilder =
    builder.WithName(name)

let withCommandDescription (name: string) (builder: SlashCommandBuilder): SlashCommandBuilder =
    builder.WithDescription(name)
    
let withCommandOption (optionBuilder: SlashCommandOptionBuilder) (builder: SlashCommandBuilder): SlashCommandBuilder =
    builder.AddOption(optionBuilder)
