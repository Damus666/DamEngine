using DamEngine;
using SDL2;

namespace DamEngine;

/// <summary>
/// A class containing the window and useful methods for it.
/// </summary>
public sealed class Window
{
    public IntPtr _sdlWindow { get; private set; }
    public IntPtr _renderer { get; private set; }
    public float _delta { get; private set; }

    private int _width = 0;
    private int _height = 0;

    /// <summary>
    /// Used internally to check if Init has been called.
    /// </summary>
    public bool hasInit { get; private set; } = false;

    /// <summary>
    /// The width of the window.
    /// </summary>
    public int width
    {
        get
        {
            RefreshSize();
            return _width;
        }

        set
        {
            _width = value;
            Resize(new vec2(_width, _height));
        }
    }

    /// <summary>
    /// The height of the window.
    /// </summary>
    public int height
    {
        get
        {
            RefreshSize();
            return _height;
        }

        set
        {
            _height = value;
            Resize(new vec2(_width, _height));
        }
    }

    /// <summary>
    /// Creates the window from given parameters.
    /// </summary>
    /// <param name="size">Width and height of the window.</param>
    /// <param name="title">Window title.</param>
    /// <param name="framerate">Max framerate.</param>
    /// <param name="flags">Flags for the window. <see cref="WindowFlag"/></param>
    public void Init(vec2 size, string title="Game Window", int framerate=60, SDL.SDL_WindowFlags flags = SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN)
    {
        _sdlWindow = SDL.SDL_CreateWindow(title,
            SDL.SDL_WINDOWPOS_CENTERED,
            SDL.SDL_WINDOWPOS_CENTERED,
            (int)size.x, (int)size.y, flags);
        _renderer = SDL.SDL_CreateRenderer(_sdlWindow, 0, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
        Console.WriteLine("Successfully created the window");
        _delta = 1000 / framerate;
        hasInit = true;
    }
    /// <summary>
    /// Changes the window title.
    /// </summary>
    /// <param name="title">The new title.</param>
    public void SetTitle(string title)
    {
        SDL.SDL_SetWindowTitle(_sdlWindow, title);
    }
    /// <summary>
    /// Obtain the current title.
    /// </summary>
    /// <returns>The title.</returns>
    public string GetTitle()
    {
        return SDL.SDL_GetWindowTitle(_sdlWindow);
    }
    /// <summary>
    /// Changes the window dimensions.
    /// </summary>
    /// <param name="newSize"></param>
    public void Resize(vec2 newSize)
    {
        SDL.SDL_SetWindowSize(_sdlWindow, (int)newSize.x, (int)newSize.y);
    }

    private void RefreshSize()
    {
        SDL.SDL_GetWindowSize(_sdlWindow, out _width, out _height);
    }
}