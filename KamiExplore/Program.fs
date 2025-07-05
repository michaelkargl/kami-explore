module Main

open System
open System.IO
open Terminal.Gui.App
open KamiExplore.FileListViewShared
open KamiExplore.MainWindow

type StatusCode =
    | Ok = 0
    | NotOk = 1

let directoryContent (path: string) : FileInfo array =
    Directory.GetFileSystemEntries(path)
    |> Array.map (fun p -> FileInfo p)
    |> Array.sortByDescending (fun item -> Directory.Exists item.FullName, item.Name.ToLower())

let window = new MainWindow()

let writeFile (outputPath: string) (fileContent: string) =
    File.WriteAllText(outputPath, fileContent)

let writeResultFile =
    let path = Path.Join(Path.GetTempPath(), "explorer_result_path.txt")
    writeFile path

let showFileSelector (directoryPath: string) : string =
    let mutable selectedItem: FileItem option = None

    Application.Init()

    let items = directoryContent directoryPath
    window.SetPaths items

    window.DirectorySelected.Add(fun directoryItem ->
        selectedItem <- Some directoryItem
        window.Log $"Selected Directory> {directoryItem.FileInfo.FullName}"
        let newItems = directoryContent directoryItem.FileInfo.FullName
        window.SetPaths newItems)

    window.FileSelected.Add(fun fileItem ->
        selectedItem <- Some fileItem
        window.Log $"Selected File> {fileItem.FileInfo.FullName}"
        window.RequestStop())

    window.NavigateToParent.Add(fun () ->
        if (selectedItem.IsSome) then
            let parent = Directory.GetParent(selectedItem.Value.FileInfo.FullName)
            window.Log $"Selected Parent> {parent.FullName}"
            let newItems = directoryContent parent.FullName
            window.SetPaths newItems)

    Application.Run(window)
    Application.Shutdown()
    
    match selectedItem with
    | Some s -> s.FileInfo.FullName
    | _ -> ""

let testPath (path: string) : bool =
    let fileExists = File.Exists(path)
    let directoryExists = not fileExists && Directory.Exists(path)
    fileExists || directoryExists

let ensurePath (path: string) : unit =
    if not (testPath path) then
        raise (FileNotFoundException($"The specified item {path} does not exist."))

[<EntryPoint>]
let main args =
    try
        let inputPath =
            match args.Length with
            | 0 -> Directory.GetCurrentDirectory()
            | _ -> args.[0]

        ensurePath inputPath

        let result =
            if File.Exists(inputPath) then
                // already a file -> pass through
                inputPath
            else
                // path is a directory -> run explorer ui
                showFileSelector inputPath

        // print to file so that the CLI can pick it up
        // writing to stdout does not work since term ui also uses stdout
        writeResultFile result
        
        (int) StatusCode.Ok
    with ex ->
        raise (ApplicationException("An unhandled error occured.", ex))
        (int) StatusCode.NotOk
