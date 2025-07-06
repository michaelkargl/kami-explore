module KamiExplore.FileListViewState

open System.Collections.Generic
open System.IO
open KamiExplore.FileSystem

type FileListViewState =
    { Items: FileItem list
      SelectedIndex: int option }

let empty: FileListViewState = {
    Items = []
    SelectedIndex = None
}

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
