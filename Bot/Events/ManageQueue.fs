module Bot.Events.ManageQueue

open Bot.Events.UserVoiceStateUpdated
open Bot.Queue
open Bot.Wrapper.Ffmpeg

let rec queueManager (queue: LockedQueue<QueueItem>) = async {
    do! Async.Sleep(1000)
    if getQueueLength queue > 0 then
        let queueItem = popFromQueue queue
        do! playIntoChannel queueItem.path queueItem.channel |> Async.AwaitTask
    return! queueManager queue
}