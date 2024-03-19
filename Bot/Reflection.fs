module Bot.Reflection

open Discord.WebSocket

let rec doHandle (options: SocketSlashCommandDataOption list) (handler: obj) client command =
    let args = List.map (fun (option: SocketSlashCommandDataOption) -> option.Value) options
    let fnType = handler.GetType()
    let methodInfo =
        fnType.GetMethods()
        |> Array.filter (fun methodInfo -> methodInfo.Name = "Invoke")
        |> Array.head
    let allArgs = args @ [client] @ [command]
    methodInfo.Invoke(handler, (List.toArray allArgs))
