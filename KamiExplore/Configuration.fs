module KamiExplore.Configuration

open System.IO

type AppConfig = { ResultFilePath: string }

let defaultConfig =
    { ResultFilePath = Path.Join(Path.GetTempPath(), "explorer_result_path.txt") }
