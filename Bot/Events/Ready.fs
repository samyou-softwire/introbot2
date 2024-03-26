module Bot.Events.Ready

open System.Diagnostics
open System.Threading.Tasks
open Discord
open Bot.Wrapper.SlashCommandBuilder
open Discord.Audio

let ready (client: IDiscordClient) (commands: BuiltCommand<obj> list) (): Task = task {
    let! guild = client.GetGuildAsync(543719732100988943UL)
    let! channel = guild.GetVoiceChannelAsync(543719732100988948UL)
    let! connection = channel.ConnectAsync()
    
    let startInfo = ProcessStartInfo()
    startInfo.CreateNoWindow <- true
    startInfo.UseShellExecute <- false
    startInfo.RedirectStandardOutput <- true
    startInfo.FileName <- "./downloaded-binaries/ffmpeg.exe"
    startInfo.WindowStyle <- ProcessWindowStyle.Hidden
    startInfo.Arguments <- $"-hide_banner -loglevel panic -i \"./intro-cache/test2.opus\" -ac 2 -f s16le -ar 48000 pipe:1"
    
    use voiceProcess = Process.Start(startInfo)
    use stream = voiceProcess.StandardOutput.BaseStream
    use voiceStream = connection.CreatePCMStream(AudioApplication.Mixed)
    do! stream.CopyToAsync(voiceStream)
    do! voiceProcess.WaitForExitAsync()
    connection.Dispose()
    
    let commandProperties =
        commands
        |> List.map (fun command -> command.properties :> ApplicationCommandProperties)
        |> List.toArray
    
    do! guild.BulkOverwriteApplicationCommandsAsync(commandProperties) |> Async.AwaitTask |> Async.Ignore
    
    ()
}