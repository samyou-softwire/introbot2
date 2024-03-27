module Bot.Queue

open System.Collections.Generic

type LockedQueue<'a> = {
    queue: Queue<'a>
    locker: Locker
}