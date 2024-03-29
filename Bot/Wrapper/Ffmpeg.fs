module Bot.Wrapper.Ffmpeg

open System.Diagnostics
open System.Threading.Tasks
open Discord.Audio
open Discord.WebSocket

let playIntoChannelWithExecutable (ffmpegPath: string) (client: IAudioClient) (path: string) (channel: SocketVoiceChannel) = task {
    
    let startInfo = ProcessStartInfo()
    startInfo.CreateNoWindow <- true
    startInfo.UseShellExecute <- false
    startInfo.RedirectStandardOutput <- true
    startInfo.FileName <- ffmpegPath
    startInfo.WindowStyle <- ProcessWindowStyle.Hidden
    startInfo.Arguments <- $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1"
    
    use voiceProcess = Process.Start(startInfo)
    use stream = voiceProcess.StandardOutput.BaseStream
    use voiceStream = client.CreatePCMStream(AudioApplication.Music)
    do! stream.CopyToAsync(voiceStream)
    do! voiceProcess.WaitForExitAsync()
}

let playIntoChannelWindows = playIntoChannelWithExecutable "./downloaded-binaries/ffmpeg.exe"

let playIntoChannelLinux = playIntoChannelWithExecutable "ffmpeg"

let playIntoChannel: IAudioClient -> string -> SocketVoiceChannel -> Task<unit> =
    let platform = System.Environment.GetEnvironmentVariable("PLATFORM")
    match platform with
    | "windows" -> playIntoChannelWindows
    | "linux" -> playIntoChannelLinux
    | _ -> failwith "no platform set"