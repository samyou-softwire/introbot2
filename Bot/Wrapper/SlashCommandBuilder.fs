module Bot.Wrapper.SlashCommandBuilder

open Discord

let newSlashCommand = SlashCommandBuilder()

let withName (name: string) (builder: SlashCommandBuilder): SlashCommandBuilder =
    builder.WithName(name)

let withDescription (name: string) (builder: SlashCommandBuilder): SlashCommandBuilder =
    builder.WithName(name)