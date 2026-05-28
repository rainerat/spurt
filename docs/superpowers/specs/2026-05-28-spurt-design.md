# Spurt Design Spec

Date: 2026-05-28  
Status: Approved for planning  
Owner: rarie

## 1) Goal

Build a lightweight personal Windows utility named `Spurt` that enables very fast temporary web searches without opening the user's main browsers and profile-selection flows.

Primary user flow:

1. Press global shortcut.
2. Type query in a blank quick search box.
3. Press Enter to open results in an embedded wrapper.
4. Read briefly (for example Google AI Overview).
5. Close wrapper and continue work.

This is intentionally not a full browser replacement.

## 2) Product Scope

### In Scope (v1)

- Windows-native desktop utility UI.
- Global shortcut with default `Ctrl+Alt`, customizable.
- Blank quick search box opens first.
- Wrapper window is created only after Enter.
- Exactly one wrapper window may exist at once.
- If wrapper already exists and a new search is submitted, reuse same wrapper and navigate to the new query.
- Default close behavior: wrapper stays open until user closes via window `X`.
- Optional setting: close wrapper on unfocus.
- Auto-start with Windows enabled by default, toggleable off.
- Hidden/advanced setting for search engine template (Google default).
- Theme mode setting: `System` (default), `Light`, `Dark`.

### Out of Scope (v1)

- Tabs, bookmarks, history manager, downloads.
- Multi-window browsing sessions.
- Long-form heavy browsing workflows.
- Browser extension support.
- Cross-platform support beyond Windows.

## 3) Technical Approach

Recommended stack:

- **App framework:** WinUI 3 on .NET.
- **Embedded web surface:** WebView2.
- **Runtime model:** single-instance resident utility with native settings and system integration.

Why this stack:

- Best match for Microsoft-native appearance and behavior.
- Good startup/tray/hotkey integration on Windows.
- Reliable modern site rendering for Google search workflows.

## 4) Architecture

Spurt runs as a single-instance host process. It initializes settings, global hotkey registration, and startup/tray behavior. The search UI and wrapper window are separate windows managed by a central controller.

High-level architecture:

- `AppHost`
  - Process bootstrap and lifetime.
  - Single-instance enforcement.
- `HotkeyService`
  - Global shortcut register/unregister and validation.
- `SearchBoxWindow`
  - Lightweight, blank input UI shown on shortcut.
- `SearchRouter`
  - Query to URL resolution based on selected engine template.
- `WrapperWindowManager`
  - Owns creation, reuse, focus, and destruction of the one allowed wrapper.
- `WrapperWindow` (WebView2 host)
  - Displays search results.
- `SettingsService`
  - Loads/saves preferences and raises change notifications.
- `StartupService`
  - Manages launch-at-login setting.
- `SettingsWindow`
  - Native settings UI.

## 5) Core Behavior and Data Flow

### Startup flow

1. App launches (Windows startup by default or manual).
2. `AppHost` loads settings.
3. `HotkeyService` registers current shortcut.
4. App stays resident and idle.

### Search flow

1. User presses shortcut (`Ctrl+Alt` default).
2. `SearchBoxWindow` opens with empty input.
3. User types query and presses Enter.
4. `SearchRouter` builds target URL from configured template.
5. `WrapperWindowManager`:
   - Creates wrapper if none exists, then navigates.
   - Reuses existing wrapper and navigates if one exists.
6. Wrapper is brought to front after navigation.

### Wrapper lifecycle rules

- Only one wrapper window can exist.
- New queries never spawn additional wrapper windows.
- Default close policy is manual close via window controls.
- If close-on-unfocus is enabled, wrapper closes when focus leaves the window.

## 6) Settings Model

Persisted user settings:

- `hotkey` (default: `Ctrl+Alt`)
- `launchOnStartup` (default: `true`)
- `closeOnUnfocus` (default: `false`)
- `searchEngineTemplate` (default: Google query URL template)
- `themeMode` (default: `System`; options: `System`, `Light`, `Dark`)

UI notes:

- Keep settings native and minimal, in a Powertoys-like clean style.
- Search engine customization can be labeled as advanced to keep v1 simple.

## 7) Error Handling and Edge Cases

- **Hotkey conflict:** do not apply invalid binding; show clear error and keep previous valid key.
- **Empty query submit:** ignore with subtle feedback; do not create wrapper.
- **WebView2 runtime unavailable:** show actionable native message and recovery steps.
- **No network:** wrapper still opens; site-level failure is shown naturally.
- **Rapid trigger/submit race:** serialize wrapper operations through manager to maintain single window invariant.
- **Startup registration blocked:** show non-blocking warning in settings and allow retry.
- **Corrupt settings file:** restore defaults, preserve backup of corrupt file, notify user.
- **Theme updates:** apply immediately to open windows; `System` mode follows OS theme changes.

## 8) Non-Functional Expectations

- Fast shortcut-to-input response.
- Low idle overhead while resident.
- Predictable single-window behavior.
- Native Windows look and feel across all surfaces.

## 9) Test Strategy

### Unit tests

- `SearchRouter` URL encoding and template substitution.
- `SettingsService` defaults, round-trip persistence, corrupt-file fallback.
- Hotkey validation logic where testable independently.

### Integration tests

- Single-instance process behavior.
- First submit creates wrapper; subsequent submits reuse and navigate.
- Setting changes propagate correctly (hotkey/theme/close-on-unfocus).

### Manual acceptance verification

- Pressing default hotkey always opens blank search box.
- Enter opens search results in wrapper.
- Existing wrapper is reused for subsequent searches.
- Default close behavior is manual window close.
- Optional close-on-unfocus works when enabled.
- Startup toggle functions correctly.
- Theme modes (`System`, `Light`, `Dark`) render and switch correctly.

## 10) V1 Acceptance Criteria

Spurt v1 is complete when all are true:

1. Default shortcut `Ctrl+Alt` opens blank quick search box.
2. Enter with non-empty query opens embedded search wrapper.
3. Wrapper is lazily created only after query submission.
4. At most one wrapper window exists at any time.
5. New searches reuse existing wrapper and navigate in place.
6. Default close policy is manual close via window `X`.
7. Close-on-unfocus option exists and works when enabled.
8. Native settings UI persists:
   - Hotkey
   - Startup behavior
   - Search engine template (Google default)
   - Theme mode (`System`, `Light`, `Dark`)
9. Utility remains focused on quick temporary searches, not full browsing.

## 11) Future Extensions (Post-v1, Optional)

- Additional engine presets UI.
- Optional instant suggestions/autocomplete.
- Better wrapper chrome controls for quick back/refresh.
- Export/import settings profile.

