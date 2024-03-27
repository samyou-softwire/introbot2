module Bot.Wrapper.Ffmpeg

open System.Diagnostics
open Discord.Audio
open Discord.WebSocket

let playIntoChannel (path: string) (channel: SocketVoiceChannel) = task {
    let! connection = channel.ConnectAsync()
    
    let startInfo = ProcessStartInfo()
    startInfo.CreateNoWindow <- true
    startInfo.UseShellExecute <- false
    startInfo.RedirectStandardOutput <- true
    startInfo.FileName <- "./downloaded-binaries/ffmpeg.exe"
    startInfo.WindowStyle <- ProcessWindowStyle.Hidden
    startInfo.Arguments <- $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1"
    
    use voiceProcess = Process.Start(startInfo)
    use stream = voiceProcess.StandardOutput.BaseStream
    use voiceStream = connection.CreatePCMStream(AudioApplication.Mixed)
    do! stream.CopyToAsync(voiceStream)
    do! voiceProcess.WaitForExitAsync()
    connection.Dispose()
}