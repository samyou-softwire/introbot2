module Bot.Queue

open System.Collections.Generic
open Bot.Wrapper.Ffmpeg

type LockedQueue<'a> = {
    queue: Queue<'a>
}

let newLockedQueue<'a> () = {
    queue = new Queue<'a>() 
}

let pushToQueue (queue: LockedQueue<'a>) (e: 'a) =
    lock (queue.queue) (fun _ ->
        queue.queue.Enqueue(e)
    )

let popFromQueue (queue: LockedQueue<'a>) =
    lock (queue.queue) (fun _ ->
        queue.queue.Dequeue()
    )
    
let getQueueLength (queue: LockedQueue<'a>) =
    queue.queue.Count