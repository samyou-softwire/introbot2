open Bot

DotNetEnv.Env.TraversePath().Load() |> ignore;

Bot.start
    |> Async.AwaitTask
    |> Async.RunSynchronously