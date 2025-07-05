module KamiExplore.FileListViewEvents

open Terminal.Gui.Input
open KamiExplore.FileListViewShared

let handleKeyPress
    (getSelectedItem: unit -> FileItem option)
    (onAction: FileListAction -> unit)
    (key: Key)
    =
    if key = Key.Enter || key = Key.CursorRight then
        getSelectedItem ()
        |> Option.iter (fun item ->
            let action =
                if item.IsDirectory then
                    DirectorySelected item
                else
                    FileSelected item

            onAction action)
    elif key = Key.CursorLeft then
        onAction NavigateToParent
    else
        ()