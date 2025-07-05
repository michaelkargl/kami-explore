module KamiExplore.App

open System.IO
open KamiExplore.FileListViewShared
open KamiExplore.MainWindow
open Terminal.Gui.App

type Arguments = { InputPath: string }

let window = new MainWindow()

let writeResultFile =
    let path = Configuration.defaultConfig.ResultFilePath
    FileSystem.writeFile path

let showFileSelector (directoryPath: string) : string =
    let mutable selectedItem: FileItem option = None

    Application.Init()

    let items = FileSystem.directoryContent directoryPath
    window.SetPaths items

    window.DirectorySelected.Add(fun directoryItem ->
        selectedItem <- Some directoryItem
        window.Log $"Selected Directory> {directoryItem.FileInfo.FullName}"

        let newItems =
            FileSystem.directoryContent directoryItem.FileInfo.FullName

        window.SetPaths newItems)

    window.FileSelected.Add(fun fileItem ->
        selectedItem <- Some fileItem
        window.Log $"Selected File> {fileItem.FileInfo.FullName}"
        window.RequestStop())

    window.NavigateToParent.Add(fun () ->
        if (selectedItem.IsSome) then
            let parent =
                Directory.GetParent(selectedItem.Value.FileInfo.FullName)

            window.Log $"Selected Parent> {parent.FullName}"
            let newItems = FileSystem.directoryContent parent.FullName
            window.SetPaths newItems)

    Application.Run(window)
    Application.Shutdown()

    match selectedItem with
    | Some s -> s.FileInfo.FullName
    | _ -> ""

let run (args: Arguments) : unit =
    FileSystem.ensurePath args.InputPath

    let result =
        if File.Exists(args.InputPath) then
            // already a file -> pass through
            args.InputPath
        else
            // path is a directory -> run explorer ui
            showFileSelector args.InputPath

    // print to file so that the CLI can pick it up
    // writing to stdout does not work since term ui also uses stdout
    writeResultFile result
