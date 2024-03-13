module Bot.Commands.Cool

open Bot.Wrapper.SlashCommandBuilder
open Bot.Wrapper.SlashCommandOptionBuilder

let coolCommandHandler (name: string) _ = $"{name} is cool!" 

let coolCommand =
    newSlashCommand
    |> withCommandName "typed cool"
    |> withCommandDescription "but will it bind?"
    |> withCommandOption (
        newStringOption
        |> withOptionName "who's cool"
        |> withOptionDescription "find out who it is!" )
    |> withHandler coolCommandHandler