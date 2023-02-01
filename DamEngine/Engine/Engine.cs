using SDL2;
using System;

namespace DamEngine;

/// <summary>
/// Main class of the engine. Contains the instance of 5 main classes (SpriteManager,Window,Camera,World,Game) and the 4 main methods.
/// To create your game call Init first, then call the Init on the Window instance.
/// You can then call Setup on the camera and world to customise them (optional).
/// After you are finish setting up the game call Loop to start it. 
/// You can manually call Quit but it will be called automatically by the user closing the winodow.
/// </summary>
public static class Engine
{
    /// <summary>
    /// Instance of <see cref="SpriteManager"/>.
    /// </summary>
    public static readonly SpriteManager spriteManager = new();
    /// <summary>
    /// Instance of <see cref="Window"/>.
    /// </summary>
    public static readonly Window window = new();
    /// <summary>
    /// Instance of <see cref="Camera"/>.
    /// </summary>
    public static readonly Camera camera = new();
    /// <summary>
    /// Instance of <see cref="World"/>.
    /// </summary>
    public static readonly World world = new();
    /// <summary>
    /// Instance of <see cref="Game"/>.
    /// </summary>
    public static readonly Game game = new();

    /// <summary>
    /// Initializate the SDL2 library.
    /// </summary>
    /// <returns>true if initialization succeded, false otherwise.</returns>
    public static bool Init()
    {
        try
        {
            SDL.SDL_SetHint(SDL.SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1");
            SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);
            Console.WriteLine("SDL2 initialized");
            return true;
        } catch
        {
            Console.WriteLine($"Error while initializing SDL2: {SDL.SDL_GetError()}");
            return false;
        }
    }
    /// <summary>
    /// Starts the game loop. Don't put any other local code below this, as this starts a while loop.
    /// Window must be initialized.
    /// </summary>
    /// <exception cref="Exception">
    /// If window is not initialized.
    /// </exception>
    public static void Loop()
    {
        if (!window.hasInit)
        {
            throw new Exception("Window must be initialized before starting the game loop.");
        }
        Console.WriteLine("Starting the game loop");
        bool running = true;
        game._OnAwake();
        DateTime loopStart = DateTime.UtcNow;
        double lastFrameTime=0;
        SDL.SDL_Event currentEvent;
        while (running)
        {
            TimeSpan t = (DateTime.UtcNow - loopStart);
            double now = t.TotalSeconds;
            ulong start = SDL.SDL_GetTicks();
                while (SDL.SDL_PollEvent(out currentEvent) != 0)
                {
                    switch (currentEvent.type)
                    {
                        case SDL.SDL_EventType.SDL_QUIT:
                            running = false;
                            Quit();
                            break;
                    }
                    game._OnEvent(currentEvent);

                }

                SDL.SDL_SetRenderDrawColor(window._renderer, 0, 0, 0,1);
                SDL.SDL_RenderClear(window._renderer);

                game._OnUpdate();
                game._OnRender();

                SDL.SDL_RenderPresent(window._renderer);

                ulong end = SDL.SDL_GetTicks();
                float elapsedMS = end - start;
            
                game._OnEndFrame(now-lastFrameTime, 1/game.deltaTime,end);
            
            if (elapsedMS < game._window._delta)
            {
                SDL.SDL_Delay((uint)(game._window._delta - elapsedMS));
            }
            lastFrameTime = now;
        }
    }
    /// <summary>
    /// Calls OnQuit on callbacks and objects then frees memory and quit the application.
    /// </summary>
    public static void Quit()
    {
        game._OnQuit();
        spriteManager._OnQuit();
        SDL.SDL_DestroyRenderer(window._renderer);
        SDL.SDL_DestroyWindow(window._sdlWindow);
        SDL.SDL_Quit();
        Console.WriteLine("Quitted application");
    }

    /// <summary>
    /// Same as 'Console.WriteLine(object o)'
    /// </summary>
    /// <param name="o">
    /// The object to debug.
    /// </param>
    public static void Debug(object o)
    {
        Console.WriteLine(o);
    }
}