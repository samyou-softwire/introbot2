module Bot.Music.Download

open System.Diagnostics

let downloadWithExecutable path url filename =
    let startInfo = ProcessStartInfo()
    startInfo.CreateNoWindow <- true
    startInfo.UseShellExecute <- false
    startInfo.FileName <- path
    startInfo.WindowStyle <- ProcessWindowStyle.Hidden
    startInfo.Arguments <- $" -P intro-cache -o {filename} -x -q {url}"
    
    using (Process.Start(startInfo)) <| fun dlProcess ->
        dlProcess.WaitForExit()
        
    ()
    
let downloadWindows =
    downloadWithExecutable "./downloaded-binaries/yt-dlp"

let download: string -> string -> unit =
    let platform = System.Environment.GetEnvironmentVariable("PLATFORM")
    match platform with
    | "windows" -> downloadWindows
    | _ -> failwith "no platform set"