# Spurt

Lightweight Windows quick-search utility built with WinUI 3 and WebView2.

## Features
- Global shortcut (`Ctrl+Alt` by default)
- Starts with a blank search box
- Reuses a single wrapper window
- Optional close-on-unfocus behavior
- Native settings for startup, theme, and engine template

## Setup
```bash
dotnet restore
dotnet build Spurt.sln
dotnet test tests/Spurt.App.Tests/Spurt.App.Tests.csproj
```

## Manual verification
1. Launch the app.
2. Press `Ctrl+Alt`.
3. Enter a query and press Enter.
4. Confirm the wrapper opens and is reused on a second query.
5. Confirm settings persist and theme toggles work.
