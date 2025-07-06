module KamiExplore.FileListViewUi

open System.Collections.ObjectModel
open Terminal.Gui.Drawing
open Terminal.Gui.ViewBase
open Terminal.Gui.Views
open System.Collections.Generic
open System.IO
open KamiExplore.FileSystem
open Terminal.Gui.Input
open KamiExplore.FileListViewShared


let createListView (itemCollection: ObservableCollection<string>) =
    new ListView(
        Width = Dim.Fill(),
        Height = Dim.Fill(),
        X = Pos.Absolute(0),
        Y = Pos.Absolute(0),
        Source = new ListWrapper<string>(itemCollection),
        BorderStyle = LineStyle.Single
    )

let updateFileListCollection
    (collection: ObservableCollection<string>)
    (state: FileListViewState.FileListViewState)
    =
    collection.Clear()
    state.Items |> List.iter (fun item -> collection.Add(item.DisplayName))

type FileListView(paths: IEnumerable<FileInfo>) as this =
    inherit View()
    let mutable currentState = FileListViewState.empty

    let directorySelectedEvent = Event<FileItem>()
    let fileSelectedEvent = Event<FileItem>()
    let navigateToParentEvent = Event<unit>()
    let lstPathsItemCollection = ObservableCollection<string>([])
    let lstPaths = createListView lstPathsItemCollection

    let getSelectedItem () =
        let index = lstPaths.SelectedItem
        FileListViewState.getSelectedItem currentState index

    let handleItemSelectionEvent =
        function
        | FileSelected fileItem -> fileSelectedEvent.Trigger(fileItem)
        | DirectorySelected dirItem -> directorySelectedEvent.Trigger(dirItem)
        | NavigateToParent -> navigateToParentEvent.Trigger()

    let updateUI () =
        updateFileListCollection (lstPathsItemCollection) (currentState)

    do
        this.BorderStyle <- LineStyle.None
        lstPaths.CanFocus <- true
        lstPaths.SetFocus() |> ignore

        lstPaths.KeyDown.Add(
            FileListViewEvents.handleKeyPress
                getSelectedItem
                handleItemSelectionEvent
        )

        lstPaths.MouseClick.Add(fun _ ->
            FileListViewEvents.handleKeyPress
                getSelectedItem
                handleItemSelectionEvent
                Key.CursorRight)

        this.SetPaths paths
        this.Add(lstPaths) |> ignore

    member public this.LstPaths = lstPaths
    member public this.DirectorySelected = directorySelectedEvent.Publish
    member public this.FileSelected = fileSelectedEvent.Publish
    member public this.NavigateToParent = navigateToParentEvent.Publish

    member public this.SetPaths(paths: IEnumerable<FileInfo>) : unit =
        currentState <- FileListViewState.fromPaths paths
        updateUI ()

    member public this.SetFocusOnListView() : unit =
        this.LstPaths.SetFocus() |> ignore
