module Bot.Wrapper.SlashCommandBuilder

open Discord

let newSlashCommand = SlashCommandBuilder()

let withCommandName (name: string) (builder: SlashCommandBuilder): SlashCommandBuilder =
    builder.WithName(name)

let withCommandDescription (name: string) (builder: SlashCommandBuilder): SlashCommandBuilder =
    builder.WithDescription(name)
    
let withCommandOption (optionBuilder: SlashCommandOptionBuilder) (builder: SlashCommandBuilder): SlashCommandBuilder =
    builder.AddOption(optionBuilder)

    
// Foo<string, Foo<string, Done>> f: string -> string -> Client

type ICommandType<'a> =
    abstract member elem: 'a

type StringCommand(e: string) =
    interface ICommandType<string> with
        member this.elem = e

type NumberCommand(e: int) =
    interface ICommandType<int> with
        member this.elem = e

let func1<'a, 'b> ((command): (ICommandType<'a>)) (handler: ('a -> string -> unit)) =
    let result = handler command.elem
    result "5"

let func2 ((command, command2): (ICommandType<'a> * ICommandType<'b>)) (handler: ('a -> 'b -> string -> unit)) =
    let result = handler command.elem
    func1 command2 result

let func3 ((command, command2, command3): (ICommandType<'a> * ICommandType<'b> * ICommandType<'c>)) (handler: ('a -> 'b -> 'c -> string -> unit)) =
    let result = handler command.elem
    func2 (command2, command3) result

let command = (StringCommand("5"), NumberCommand(5))

let handler (name: string) (age: int) (context: string) =
    ()
    
func2 command handler