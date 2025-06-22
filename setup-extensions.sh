#!/bin/zsh

# Install all recommended VS Code extensions for ArtlessHockey

extensions=(
  ms-dotnettools.csharp
  ms-dotnettools.csdevkit
  ms-dotnettools.vscode-dotnet-runtime
  formulahendry.dotnet-test-explorer
  sonarsource.sonarlint-vscode
  ms-dotnettools.vscode-dotnet-pack
  icsharpcode.ilspy-vscode
  jchannon.csharpextensions
  bradgashler.htmltagwrap
  ms-vscode.test-adapter-converter
  jmrog.vscode-nuget-package-manager
  aliasadidev.nugetpackagemanagergui
  humao.rest-client
  rangav.vscode-thunder-client
  42crunch.vscode-openapi
  ms-vscode.vscode-yaml
  eamodio.gitlens
  github.vscode-pull-request-github
  github.copilot
  ms-vsliveshare.vsliveshare
  editorconfig.editorconfig
  esbenp.prettier-vscode
  ms-vscode.vscode-markdown
  ms-vscode.hexeditor
  snyk-security.snyk-vulnerability-scanner
  github.vscode-github-actions
  visualstudiotoolsforunity.vstuc
  tobiah.unity-tools
  aaron-bond.better-comments
  streetsidesoftware.code-spell-checker
  formulahendry.code-runner
  alefragnani.bookmarks
  oderwat.indent-rainbow
)

for ext in $extensions; do
  code --install-extension $ext
done