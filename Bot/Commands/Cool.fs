module Bot.Commands.Cool

open Bot.Wrapper.SlashCommandBuilder
open Bot.Wrapper.SlashCommandOptionBuilder

let coolCommandHandler (name: string) _ _ = task { return $"{name} is cool!" } 

let coolCommand =
    newSlashCommand()
    |> withCommandName "cool"
    |> withCommandDescription "but will it bind?"
    |> withCommandOption (
        newStringOption()
        |> withOptionName "who"
        |> withOptionDescription "find out who it is!" )
    |> withHandler coolCommandHandler