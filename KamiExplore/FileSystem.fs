module KamiExplore.FileSystem

open System.IO

type FileItem =
    { FileInfo: FileInfo
      DisplayName: string
      IsDirectory: bool }

let testPath (path: string) : bool =
    let fileExists = File.Exists(path)
    let directoryExists = not fileExists && Directory.Exists(path)
    fileExists || directoryExists

let directoryContent (path: string) : FileInfo array =
    Directory.GetFileSystemEntries(path)
    |> Array.map (fun p -> FileInfo p)
    |> Array.sortByDescending (fun item ->
        Directory.Exists item.FullName, item.Name.ToLower())

let ensurePath (path: string) : unit =
    if not (testPath path) then
        raise (
            FileNotFoundException($"The specified item {path} does not exist.")
        )

let writeFile (outputPath: string) (fileContent: string) =
    File.WriteAllText(outputPath, fileContent)

let createFileItem (fileInfo: FileInfo) : FileItem =
    let isDirectory = Directory.Exists(fileInfo.FullName)

    let displayName =
        if isDirectory then
            $"📁 {fileInfo.Name}/"
        else
            $"📄 {fileInfo.Name}"

    { FileInfo = fileInfo
      DisplayName = displayName
      IsDirectory = isDirectory }

let createFileItemFromFileSystemInfo (item: FileSystemInfo): FileItem =
    let item = FileInfo item.FullName
    createFileItem item
