# Flood Rush - GitHub Copilot Instructions

## Project Overview

Flood Rush is a puzzle game where players race against the clock to link a start valve to one or more endpoints by placing the right pipe pieces. Players must think fast, place smart, and complete the flow before water surges through their unfinished pipeline.

## Game Mechanics

- **Grid-based gameplay**: The game uses a grid where pipe pieces are placed
- **Start point**: A pipe enters the grid from one side (the start valve)
- **End point(s)**: One or more endpoints that players must connect to
- **Objective**: Complete the pipeline between start and end points before time runs out
- **Time pressure**: Players work against the clock to complete the connection

## Technology Stack

- **.NET 10**: Latest .NET framework
- **.NET MAUI**: Cross-platform UI framework for building native apps
- **.NET Aspire**: Application orchestration and observability framework
- **Target Platforms**: Android, iOS, macOS (Catalyst), Windows
- **Language Features**: C# 13, Nullable reference types enabled, Implicit usings enabled

## Project Structure

```
src/
├── HexMaster.FloodRush.App/              # Main MAUI application
│   ├── Platforms/                         # Platform-specific code
│   │   ├── Android/                       # Android-specific implementations
│   │   ├── iOS/                           # iOS-specific implementations
│   │   ├── MacCatalyst/                   # macOS-specific implementations
│   │   └── Windows/                       # Windows-specific implementations
│   ├── Resources/                         # Application resources
│   │   ├── AppIcon/                       # App icons
│   │   ├── Fonts/                         # Custom fonts
│   │   ├── Images/                        # Image assets
│   │   ├── Splash/                        # Splash screens
│   │   └── Styles/                        # XAML styles
│   ├── App.xaml(.cs)                      # Application entry point
│   ├── AppShell.xaml(.cs)                 # Shell navigation
│   ├── MainPage.xaml(.cs)                 # Main page
│   └── MauiProgram.cs                     # MAUI app configuration
└── Aspire/
    └── HexMaster.FloodRush.Aspire/
        ├── AppHost/                       # Aspire orchestration host
        └── ServiceDefaults/               # Shared Aspire configuration
```

## Coding Guidelines

### General Principles

1. **Namespace**: Use `HexMaster.FloodRush.*` as the base namespace
2. **Nullable Reference Types**: Always enabled - handle nullability explicitly
3. **Implicit Usings**: Enabled - avoid redundant using statements
4. **Code Style**: Follow standard C# conventions and .NET design guidelines

### MAUI-Specific Guidelines

1. **XAML Source Generation**: Project uses `MauiXamlInflator.SourceGen` for improved build performance
2. **View-ViewModel Separation**: Follow MVVM pattern where applicable
3. **Platform-Specific Code**: Use conditional compilation or dependency injection for platform-specific implementations
4. **Resources**: 
   - Place images in `Resources/Images/`
   - Place fonts in `Resources/Fonts/`
   - Define shared styles in `Resources/Styles/`
5. **Navigation**: Use Shell navigation via `AppShell.xaml`

### Game Development Considerations

When working on game features:

1. **Grid System**: Consider efficient grid rendering and management
2. **Pipe Logic**: Implement clear data structures for pipe connections
3. **Validation**: Check if pipes are correctly connected between start and endpoints
4. **Timer/Clock**: Implement countdown mechanics for the time pressure element
5. **Performance**: Optimize for smooth gameplay across all platforms
6. **Touch/Click Input**: Handle both touch (mobile) and mouse (desktop) inputs
7. **Visual Feedback**: Provide clear visual indicators for valid/invalid pipe placements
8. **State Management**: Track game state (in progress, won, lost, etc.)

### Aspire Integration

1. **Orchestration**: The AppHost project manages the MAUI app lifecycle
2. **Service Discovery**: Use Aspire for service discovery if backend services are added
3. **Observability**: Leverage Aspire's built-in telemetry and monitoring
4. **Configuration**: Store environment-specific settings in `appsettings.json`

## Architecture Recommendations

### Recommended Patterns

1. **MVVM (Model-View-ViewModel)**: For UI logic separation
2. **Command Pattern**: For user actions and undo/redo functionality
3. **State Pattern**: For managing game states (menu, playing, paused, game over)
4. **Factory Pattern**: For creating different pipe piece types
5. **Repository Pattern**: If adding data persistence for high scores or saved games

### Suggested Project Organization

Consider organizing the MAUI app code into:

```
HexMaster.FloodRush.App/
├── Models/              # Game entities (Grid, Pipe, Cell, etc.)
├── ViewModels/          # View models for pages
├── Views/               # XAML pages and controls
├── Services/            # Game logic services (GameEngine, ScoreService, etc.)
├── Controls/            # Custom controls (GridView, PipeControl, etc.)
├── Converters/          # Value converters for XAML bindings
└── Helpers/             # Utility classes
```

## Testing Considerations

1. **Unit Tests**: Test game logic independently of UI
2. **Platform Testing**: Test on all target platforms (Android, iOS, Windows, macOS)
3. **Performance Testing**: Ensure smooth gameplay on lower-end devices
4. **Touch Targets**: Verify adequate touch target sizes on mobile devices

## Common Tasks

### Adding a New Pipe Type

1. Define the pipe model with connection points
2. Create visual representation (SVG or image)
3. Add to Resources and ensure proper sizing
4. Implement connection logic
5. Add to pipe selection/placement UI

### Adding a New Game Level

1. Define grid configuration
2. Specify start and end point locations
3. Set time limit
4. Configure available pipe types
5. Test connectivity possibilities

### Platform-Specific Features

When adding platform-specific code:

- Use `#if ANDROID`, `#if IOS`, `#if WINDOWS`, `#if MACCATALYST` directives
- Or use dependency injection with platform-specific implementations in `Platforms/{Platform}/`
- Register platform services in `MauiProgram.cs` using conditional compilation

## Build and Debug

- **Debug Mode**: Includes logging via `Microsoft.Extensions.Logging.Debug`
- **Aspire Dashboard**: Run the AppHost project to access the Aspire dashboard
- **Hot Reload**: Supported for XAML and C# during development

## Version Information

- **.NET Version**: 10.0
- **MAUI Version**: 10.0.41
- **Minimum Platform Versions**:
  - Android: API 21
  - iOS: 15.0
  - macOS Catalyst: 15.0
  - Windows: 10.0.17763.0

## Additional Context

When suggesting features or fixes, consider:

- Cross-platform compatibility across all supported platforms
- Mobile-first design with touch interactions as primary input
- Responsive layouts for different screen sizes
- Performance on mobile devices
- Battery and resource consumption
- Offline gameplay capability
- Accessibility features (color contrast, screen readers, etc.)
