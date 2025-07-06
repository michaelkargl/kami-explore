# Kex

A tiny simplistic terminal file browser for use in CLI environments

## Use

```sh
cd kami-explore/KamiExplore
# shows help 
dotnet run

# runs explorer from root directory and saves the
# selection to the provided output file
dotnet run / --outputFile /tmp/kex_selection.txt
```

## Use in Powershell

```pwsh
# $PROFILE.CurrentUserCurrentHost
Function Invoke-Explorer {
    $resultFile = Join-Path ([System.IO.Path]::GetTempPath()) "kex_result_path.txt"
    
    dotnet run -- $PWD --outputFile $resultFile
    if ( -not (Test-Path -PathType Leaf "$resultFile" )) {
        return
    }
    
    $resultPath = Get-Content "$resultFile"
    $isDirectory = Test-Path -PathType Container "$resultPath"
    $isFile = Test-Path -PathType Leaf "$resultPath"
    if ($isDirectory) {
        Set-Location $resultPath
    } else if ( $isFile ) {
        Invoke-Item $resultPath
    }
}
```
