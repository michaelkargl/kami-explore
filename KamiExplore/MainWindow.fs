module KamiExplore.MainWindow

open System.Collections.Generic
open System.IO
open Terminal.Gui.App
open Terminal.Gui.Drawing
open Terminal.Gui.ViewBase
open Terminal.Gui.Views
open KamiExplore.FileListView
open KamiExplore.FileListViewShared
open KamiExplore.LogView


type MainWindow() as this =
    inherit Window()
    let directorySelectedEvent: Event<FileItem> = Event<FileItem>()
    let fileSelectedEvent: Event<FileItem> = Event<FileItem>()
    let navigateToParentEvent: Event<unit> = Event<unit>()

    let fileListView =
        new FileListView(
            [],
            X = Pos.Absolute(0),
            Y = Pos.Absolute(0),
            Width = Dim.Fill(),
            Height = Dim.Percent(80)
        )

    let logView =
        new LogView(
            X = Pos.Left(fileListView),
            Y = Pos.Bottom(fileListView),
            Height = Dim.Fill(),
            Width = Dim.Fill(),
            BorderStyle = LineStyle.Single,
            Title = "Log"
        )

    let setKeyboardFocusOnFileList: unit =
        fileListView.CanFocus <- true
        fileListView.SetFocusOnListView()
 
    do
        this.Title <- $"Explorer ({Application.QuitKey} to quit)"
        fileListView.DirectorySelected.Add(directorySelectedEvent.Trigger)
        fileListView.FileSelected.Add(fileSelectedEvent.Trigger)
        fileListView.NavigateToParent.Add(navigateToParentEvent.Trigger)
        setKeyboardFocusOnFileList
        this.Add(fileListView, logView)
        

    member public this.DirectorySelected = directorySelectedEvent.Publish
    member public this.FileSelected = fileSelectedEvent.Publish
    member public this.NavigateToParent = navigateToParentEvent.Publish

    member public this.SetPaths(paths: IEnumerable<FileInfo>) : unit =
        fileListView.SetPaths(paths)

    member public this.Log(line: string) : unit = logView.Log(line)
