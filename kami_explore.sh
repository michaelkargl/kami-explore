#!/bin/bash

_outputFile='/tmp/kex-file.txt'
_inputPath="${1:-$PWD}"

KamiExplore --outputFile "$_outputFile" "$_inputPath"
item=$(cat "$_outputFile")

if [[ -d "$item" ]]; then
  cd "$item"
else
  code "$item"
fi