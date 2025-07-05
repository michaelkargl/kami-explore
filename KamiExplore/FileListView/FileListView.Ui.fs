module KamiExplore.FileListViewUi

open System.Collections.ObjectModel
open Terminal.Gui.Drawing
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

let createListView (itemCollection: ObservableCollection<string>)  =
    new ListView(
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            X = Pos.Absolute(0),
            Y = Pos.Absolute(0),
            Source = new ListWrapper<string>(itemCollection),
            BorderStyle = LineStyle.Single
        )

let updateFileListCollection (collection: ObservableCollection<string>) (state: FileListViewState.FileListViewState) =
    collection.Clear()
    state.Items |> List.iter (fun item -> collection.Add(item.DisplayName))