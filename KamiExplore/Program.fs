module Main

open System
open KamiExplore

type StatusCode =
    | Ok = 0
    | NotOk = 1

[<EntryPoint>]
let main args =
    try
        let options = AppConfiguration.parseCliArgs args
        App.run options
        (int) StatusCode.Ok
    with ex ->
        raise (ApplicationException("An unhandled error occured.", ex))
        (int) StatusCode.NotOk
