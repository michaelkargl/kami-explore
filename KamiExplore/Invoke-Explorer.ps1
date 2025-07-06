
# ------------------------------------------------------------------------------
# 1. Open your powershell profile with your favorite editor
# 2. Add the following code and
# 3. adjust the $KexExec parameter to point to your installation
# ------------------------------------------------------------------------------

Function Invoke-Explorer {
  Param(
    [Parameter(Mandatory = $false, Position = 0)]
    [string] $Path = $PWD,
    # --------------------------------------------------------------------------
    [Parameter()]
    [string] $KexExec = '/home/michael/workspace/fsharp-space/kami-explore/KamiExplore/bin/Debug/net9.0/KamiExplore'
    # --------------------------------------------------------------------------
  )

  $resultFile = Join-Path ([System.IO.Path]::GetTempPath()) "kex_result_path.txt" 
  
  # run the explorer and store the selected path in the $resultFile file
  & $KexExec $Path --outputFile $resultFile
  if ( -not (Test-Path -PathType Leaf $resultFile) ) {
    return
  }
  
  $resultPath = Get-Content $resultFile
  $isDirectory = Test-Path -PathType Container $resultPath
  $isFile = Test-Path -PathType Leaf "$resultPath"

  # react on whether the file is a file or directory
  if ($isDirectory) {
    Write-Host "Directory: $resultPath"
    Set-Location $resultPath
  } elseif ( $IsFile ) {
    Write-Host "File: $resultPath"
    Invoke-Item $resultPath
  }
}

# create a short alias for ease of use
New-Alias -Name kex -Value Invoke-Explorer