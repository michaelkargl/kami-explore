# Kex

A tiny simplistic terminal file browser for use in CLI environments

![](./termtosvg_recording.svg)

## Setup for Powershell

1. Open your powershell profile with your favorite editor (`$PROFILE.CurrentUserCurrentHost`)
2. Add the code from [Invoke-Explorer.ps1]
3. Adjust the `$KexExec` parameter to point to your installation

[Invoke-Explorer.ps1]: ./KamiExplore/Invoke-Explorer.ps1

## Use

```sh
# enter powershell session
pwsh
# runs explorer from current directory
kex
# runs explorer from /opt dir
kex /opt
```