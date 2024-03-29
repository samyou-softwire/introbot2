module Bot.Wrapper.Ffmpeg

open System.Diagnostics
open System.Threading.Tasks
open Discord.Audio
open Discord.WebSocket

let playIntoChannelWithExecutable (ffmpegPath: string) (path: string) (channel: SocketVoiceChannel) = task {
    let! connection = channel.ConnectAsync()
    
    let startInfo = ProcessStartInfo()
    startInfo.CreateNoWindow <- true
    startInfo.UseShellExecute <- false
    startInfo.RedirectStandardOutput <- true
    startInfo.FileName <- ffmpegPath
    startInfo.WindowStyle <- ProcessWindowStyle.Hidden
    startInfo.Arguments <- $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1"
    
    use voiceProcess = Process.Start(startInfo)
    use stream = voiceProcess.StandardOutput.BaseStream
    use voiceStream = connection.CreatePCMStream(AudioApplication.Music)
    do! stream.CopyToAsync(voiceStream)
    do! voiceProcess.WaitForExitAsync()
    connection.Dispose()
}

let playIntoChannelWindows = playIntoChannelWithExecutable "./downloaded-binaries/ffmpeg.exe"

let playIntoChannelLinux = playIntoChannelWithExecutable "./downloaded-binaries/ffmpeg"

let playIntoChannel: string -> SocketVoiceChannel -> Task<unit> =
    let platform = System.Environment.GetEnvironmentVariable("PLATFORM")
    match platform with
    | "windows" -> playIntoChannelWindows
    | "linux" -> playIntoChannelLinux
    | _ -> failwith "no platform set"