module Bot.Music.Download
    
let downloadWindows (url: string) =
    printf "windows"
    ()

let download (url: string) =
    let platform = System.Environment.GetEnvironmentVariable("PLATFORM")
    match platform with
    | "windows" -> downloadWindows url
    | _ -> ()