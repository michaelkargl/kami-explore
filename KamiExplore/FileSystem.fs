module KamiExplore.FileSystem

open System.IO

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
