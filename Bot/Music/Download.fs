module Bot.Music.Download

open System.Diagnostics
open System.Threading.Tasks

let downloadWithExecutable path url filename onStart onEnd = task {
    let startInfo = ProcessStartInfo()
    startInfo.CreateNoWindow <- true
    startInfo.UseShellExecute <- false
    startInfo.FileName <- path
    startInfo.WindowStyle <- ProcessWindowStyle.Hidden
    startInfo.Arguments <- $" -P intro-cache -o {filename} -x -q {url}"
    
    do! onStart
    printf "onstart"
    
    use dlProcess = Process.Start(startInfo)
    dlProcess.WaitForExit()
    
    do! onEnd
    printf "onend"
    
    ()
}
    
let downloadWindows =
    downloadWithExecutable "./downloaded-binaries/yt-dlp"
    
let downloadLinux =
    downloadWithExecutable "./downloaded-binaries/yt-dlp"

let download: string -> string -> Task<unit> -> Task<unit> -> Task<unit> =
    let platform = System.Environment.GetEnvironmentVariable("PLATFORM")
    match platform with
    | "windows" -> downloadWindows
    | "linux" -> downloadLinux
    | _ -> failwith "no platform set"