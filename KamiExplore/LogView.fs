module KamiExplore.LogView

open System.Collections.ObjectModel
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

type LogView() as this =
    inherit View()

    let lstLogItemCollection = ObservableCollection<string>([])
    
    do
        this.Add(
            new ListView(
                X = Pos.Absolute(0),
                Y = Pos.Absolute(0),
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                Source = new ListWrapper<string>(lstLogItemCollection)
            )
        )
        |> ignore

    member public this.Log(line: string) : unit = lstLogItemCollection.Insert(0, line)
