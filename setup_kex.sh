#!/bin/bash

if [ $# -lt 2 ]; then
  echo "Usage: ./setup_kex.sh --profile <profile-path>"
  echo "Example Zsh: ./setup_kex.sh --profile ~/.zshrc"
  exit 1
fi

_script_dir=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" &>/dev/null && pwd)
_script_path="$_script_dir/kami_explore.sh"
_profile="$2"

echo '' | tee -a "$_profile"
echo '# Kami Explore' | tee -a "$_profile"
echo "alias kex='source $_script_path'" | tee -a "$_profile"
