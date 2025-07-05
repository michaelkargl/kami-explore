module Main

open System
open System.IO
open KamiExplore.App

type StatusCode =
    | Ok = 0
    | NotOk = 1

let parseArguments (args: string array) : Arguments =
    let inputPath =
        match args.Length with
        | 0 -> Directory.GetCurrentDirectory()
        | _ -> args.[0]

    { InputPath = inputPath }

[<EntryPoint>]
let main args =
    try
        let arguments = parseArguments args
        run arguments
        (int) StatusCode.Ok
    with ex ->
        raise (ApplicationException("An unhandled error occured.", ex))
        (int) StatusCode.NotOk
