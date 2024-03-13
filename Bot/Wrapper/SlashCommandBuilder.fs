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
    
// (string -> string -> number) -> client -> string

// newBuilder  -- create the builder which can bind a IDiscordClient -> string command
// the object has no methods
// hence there can be a bind function which takes
// builder: CommandBuilder<'a> f: 'a

let a: string -> int -> IDiscordClient -> string = ()

let bind: (string -> int -> IDiscordClient -> string) -> unit = ()

type CoolBuilder<'a> = CoolBuilderValue

let bindx (builder: CoolBuilder<'a>) (handler: 'a) = ()

let newBuilder: CoolBuilder<IDiscordClient -> string> = CoolBuilderValue

let withString<'a> (builder: CoolBuilder<'a>): CoolBuilder<string -> 'a> = CoolBuilderValue

let withNumber<'a> (builder: CoolBuilder<'a>): CoolBuilder<int -> 'a> = CoolBuilderValue

let builderOfCool = withNumber (withString newBuilder)

let handler x y client = "5"

bindx builderOfCool handler