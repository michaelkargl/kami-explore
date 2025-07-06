module KamiExplore.App

open System.IO
open KamiExplore.AppConfiguration
open KamiExplore.FileSystem
open KamiExplore.MainWindow
open Terminal.Gui.App

let window = new MainWindow()

let runUiApplication (initializer: unit -> unit) =
    Application.Init()
    initializer ()
    Application.Run(window)
    Application.Shutdown()

let showFileSelector (directoryPath: string) : string =
    let mutable selectedItem: FileItem option = None

    let updateSelectedItem (item: FileItem) = selectedItem <- Some item

    let directorySelectedHandler =
        AppEvents.handleDirectorySelected updateSelectedItem window

    let fileSelectedHandler =
        AppEvents.handleFileSelected updateSelectedItem window

    let parentSelectedHandler () =
        AppEvents.handleParentSelected updateSelectedItem selectedItem window

    runUiApplication (fun () ->
        let items = FileSystem.directoryContent directoryPath
        window.SetPaths items
        window.DirectorySelected.Add(directorySelectedHandler)
        window.FileSelected.Add(fileSelectedHandler)
        window.NavigateToParent.Add(parentSelectedHandler))

    match selectedItem with
    | Some s -> s.FileInfo.FullName
    | _ -> ""

let run (argv: Options) : unit =
    FileSystem.ensurePath argv.inputPath
    
    let result =
        if File.Exists(argv.inputPath) then
            // already a file -> pass through
            argv.inputPath
        else
            // path is a directory -> run explorer ui
            showFileSelector argv.inputPath

    // print to file so that the CLI can pick it up
    // writing to stdout does not work since term ui also uses stdout
    FileSystem.writeFile argv.outputFile result
