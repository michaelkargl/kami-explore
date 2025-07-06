module KamiExplore.AppEvents

open System
open System.IO
open KamiExplore
open KamiExplore.FileSystem
open KamiExplore.MainWindow

let handleDirectorySelected
    (setSelectedItem: FileItem -> unit)
    (window: MainWindow)
    (directoryItem: FileItem)
    : unit =
    setSelectedItem directoryItem

    try
        let newItems =
            FileSystem.directoryContent directoryItem.FileInfo.FullName

        window.Log $"Selected Directory> {directoryItem.FileInfo.FullName}"
        window.SetPaths newItems
    with
    | :? UnauthorizedAccessException as ex ->
        window.Log $"Unauthorized to access: {directoryItem.FileInfo.FullName}"
    | ex ->
        window.Log $"Acess failed: {directoryItem.FileInfo.FullName}"
        window.Log ex.Message

let handleFileSelected
    (setSelectedItem: FileItem -> unit)
    (window: MainWindow)
    (fileItem: FileItem)
    : unit =
    setSelectedItem (fileItem)
    window.Log $"Selected File> {fileItem.FileInfo.FullName}"
    window.RequestStop()

let handleParentSelected
    (setSelectedItem: FileItem -> unit)
    (selectedItem: FileItem option)
    (window: MainWindow)
    : unit =
    if (selectedItem.IsSome) then
        try
            let parent =
                Directory.GetParent(selectedItem.Value.FileInfo.FullName)

            let parentItem = createFileItemFromFileSystemInfo parent
            handleDirectorySelected setSelectedItem window parentItem
        with ex ->
            window.Log $"Access failed> {selectedItem.Value.FileInfo.FullName}"
            window.Log $"Cause> {ex.Message}"
