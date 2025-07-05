module KamiExplore.FileListViewShared

open System.IO

type FileItem =
    { FileInfo: FileInfo
      DisplayName: string
      IsDirectory: bool }

type FileListAction =
    | FileSelected of FileItem
    | DirectorySelected of FileItem
    | NavigateToParent
