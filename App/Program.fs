open Bot

Bot.start
    |> Async.AwaitTask
    |> Async.RunSynchronously