module Bot.Reflection

open System
open System.Reflection
open Discord.WebSocket
open Microsoft.FSharp.Core

let isOptionType (_type: Type) =
    _type.Name.Contains("Option")

let rec doHandle (options: SocketSlashCommandDataOption list) (handler: obj) client command =
    let args =
        options |> List.map (fun (option: SocketSlashCommandDataOption) -> option.Value)
    
    let fnType = handler.GetType()
    let methodInfo =
        fnType.GetMethods()
        |> Array.filter (fun methodInfo -> methodInfo.Name = "Invoke")
        |> Array.head
        
    let correctArg (i: int) (p: ParameterInfo): obj =
        if i < args.Length then
            args[i]
        else
            null
        
    let parameters = methodInfo.GetParameters()
    let fixedParams =
        parameters
        |> Array.take(parameters.Length - 2)
        |> Array.rev
        
    let correctArgs =
        Array.mapi correctArg fixedParams
        |> List.ofArray
        |> List.rev
    
    let allArgs = correctArgs @ [client] @ [command]
    methodInfo.Invoke(handler, (List.toArray allArgs))
