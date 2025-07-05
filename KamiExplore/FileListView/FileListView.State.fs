module KamiExplore.FileListViewState

open System.Collections.Generic
open System.IO
open KamiExplore.FileListViewShared

type FileListViewState =
    { Items: FileItem list
      SelectedIndex: int option }

let empty: FileListViewState = {
    Items = []
    SelectedIndex = None
}

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

let fromPaths (paths: IEnumerable<FileInfo>) : FileListViewState =
    { Items = paths |> Seq.map createFileItem |> List.ofSeq
      SelectedIndex = None }

let getSelectedItem
    (state: FileListViewState)
    (index: int)
    : FileItem option =
    if index >= 0 && index < state.Items.Length then
        Some state.Items.[index]
    else
        None
