module KamiExplore.AppConfiguration

open System
open CommandLine

type Options =
    { [<Value(0,
              MetaName = "inputPath",
              Required = true,
              HelpText = "The path to browse using the explorer.")>]
      inputPath: string
      [<Option('o',
               "outputFile",
               Required = true,
               HelpText = "Output file path.")>]
      outputFile: string
      [<Option('h',
               "help",
               Required = false,
               HelpText = "Prints the application help")>]
      help: bool }

let parseCliArgs (args: string array): Options =
    let argv = CommandLine.Parser.Default.ParseArguments<Options>(args)
    match argv with
    | :? Parsed<Options> as parsed -> parsed.Value
    | ex -> raise (ArgumentException "Unable to parse command line params")
