﻿module Bot.Music.Download

open System.Diagnostics
open System.Threading.Tasks

let launchProcess (startInfo: ProcessStartInfo) = async {
    use dlProcess = Process.Start(startInfo)
    do! dlProcess.WaitForExitAsync() |> Async.AwaitTask
}

let downloadWithExecutable path url filename = task {
    let startInfo = ProcessStartInfo()
    startInfo.CreateNoWindow <- true
    startInfo.UseShellExecute <- false
    startInfo.FileName <- path
    startInfo.WindowStyle <- ProcessWindowStyle.Hidden
    startInfo.Arguments <- $" -P intro-cache -o {filename} -x -q {url}"
    
    do! launchProcess startInfo
    
    ()
}
    
let downloadWindows =
    downloadWithExecutable "./downloaded-binaries/yt-dlp"
    
let downloadLinux =
    downloadWithExecutable "./downloaded-binaries/yt-dlp"

let download: string -> string -> Task<unit> =
    let platform = System.Environment.GetEnvironmentVariable("PLATFORM")
    match platform with
    | "windows" -> downloadWindows
    | "linux" -> downloadLinux
    | _ -> failwith "no platform set"