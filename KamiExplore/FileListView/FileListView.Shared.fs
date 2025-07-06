module KamiExplore.FileListViewShared

open KamiExplore.FileSystem

type FileListAction =
    | FileSelected of FileItem
    | DirectorySelected of FileItem
    | NavigateToParent
