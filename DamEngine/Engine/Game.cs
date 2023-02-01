using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace DamEngine;
/// <summary>
/// The main game class.
/// </summary>
public sealed class Game
{
    /// <summary>
    /// Internal.
    /// </summary>
    public Window _window = Engine.window;
    /// <summary>
    /// Internal.
    /// </summary>
    public World _world = Engine.world;
    //public SDL_gfx.FPSmanager _fpsmanager;
    /// <summary>
    /// How many frames since the game started.
    /// </summary>
    public int frameCount { get; private set; }
    /// <summary>
    /// Time between new and old frame.
    /// </summary>
    public float deltaTime { get; private set; }
    /// <summary>
    /// Current event from the event loop.
    /// </summary>
    public SDL.SDL_Event currentEvent { get; private set; }
    /// <summary>
    /// 1/deltaTime. The frame rate of the game.
    /// </summary>
    public float frameRate { get; private set; }
    /// <summary>
    /// How many ticks since the game started.
    /// </summary>
    public float ticks { get; private set; }
    private Action? updateCallback=null;
    private Action? gizmosCallback = null;
    private Action? quitCallback = null;
    private Action? eventCallback = null;
    private Action? awakeCallback = null;
    private Dictionary<string,Timer> timers = new ();
    private Dictionary<SDL.SDL_Keycode, bool> keys = new();
    /// <summary>
    /// Binds the callbacks.
    /// </summary>
    /// <param name="updateCallback">Called every frame.</param>
    /// <param name="eventCallback">Called for every event.</param>
    /// <param name="awakeCallback">Called when the game starts.</param>
    /// <param name="quitCallback">Called when the game quits.</param>
    /// <param name="gizmosCallback">Called after entities rendering.</param>
    public void Bind(Action? updateCallback=null,Action? eventCallback=null, Action? awakeCallback=null, Action? quitCallback=null, Action? gizmosCallback=null)
    {
        this.updateCallback = updateCallback;
        this.eventCallback = eventCallback;
        this.awakeCallback = awakeCallback;
        this.quitCallback = quitCallback;
        this.gizmosCallback = gizmosCallback;
    }
    /// <summary>
    /// Creates a new timer.
    /// </summary>
    /// <param name="name">The timer name.</param>
    /// <param name="durationMS">The duration in ms.</param>
    /// <param name="onEndFunction">The action when the timer ends.</param>
    /// <param name="startActive">Should the timer be activated immediately.</param>
    /// <returns>The created timer.</returns>
    public Timer CreateTimer(string name, int durationMS, Action<Timer> onEndFunction, bool startActive)
    {
        Timer t = new Timer(durationMS, onEndFunction, startActive);
        timers[name] = t;
        return t;
    }
    /// <summary>
    /// Obtain the timer of a name if any.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The timer, if found.</returns>
    public Timer? GetTimer(string name)
    {
        if (timers.ContainsKey(name))
        {
            return timers[name];
        }
        return null;
    }
    /// <summary>
    /// What button is currently pressed.
    /// </summary>
    /// <returns>Int representing the pressed button.</returns>
    public int MouseState()
    {
        return (int)SDL.SDL_GetMouseState(out _, out _);
    }
    /// <summary>
    /// Mouse position relative to the window.
    /// </summary>
    /// <returns>The position.</returns>
    public vec2 AbsoluteMousePosition()
    {
        int x, y;
        SDL.SDL_GetMouseState(out x, out y);
        return new vec2(x, y);
    }
    /// <summary>
    /// Mouse position relative to the camera.
    /// </summary>
    /// <returns>The position.</returns>
    public vec2 RelativeMousePosition()
    {
        int x, y;
        SDL.SDL_GetMouseState(out x, out y);
        return Engine.camera.ScreenToWorld(new vec2(x, y));
    }
    /// <summary>
    /// Is the current event key of a <see cref="KeyCode"/>
    /// </summary>
    /// <param name="keyCode">The <see cref="KeyCode"/></param>
    /// <returns>true or false.</returns>
    public bool IsKeyEvent(SDL.SDL_Keycode keyCode)
    {
        if (currentEvent.type == SDL.SDL_EventType.SDL_KEYUP || currentEvent.type == SDL.SDL_EventType.SDL_KEYDOWN)
        {
            return currentEvent.key.keysym.sym == keyCode;
        }
        return false;
    }
    /// <summary>
    /// Is a key with a <see cref="KeyCode"/> pressed/hold?
    /// </summary>
    /// <param name="keyCode">The <see cref="KeyCode"/></param>
    /// <returns>true or false.</returns>
    public bool GetKey(SDL.SDL_Keycode keyCode)
    {
        if (!keys.ContainsKey(keyCode)) { keys[keyCode] = false; return false; }
        return keys[keyCode];
    }
    /// <summary>
    /// Internal
    /// </summary>
    /// <param name="elapsed"></param>
    /// <param name="fps"></param>
    /// <param name="endticks"></param>
    public void _OnEndFrame(double elapsed,float fps,ulong endticks)
    {
        //deltaTime = SDL_gfx.SDL_framerateDelay(ref _fpsmanager);
        deltaTime = (float)elapsed;
        frameRate = fps;
        frameCount++;
        ticks = endticks;
    }
    /// <summary>
    /// Internal.
    /// </summary>
    public void _OnAwake()
    {
        _world._OnAwake();
        awakeCallback?.Invoke();
    }
    /// <summary>
    /// Internal.
    /// </summary>
    public void _OnUpdate()
    {
        foreach (Timer t in timers.Values)
        {
            t._Update();
        }
        _world._OnUpdate();
        updateCallback?.Invoke();
    }
    /// <summary>
    /// Internal.
    /// </summary>
    public void _OnRender()
    {
        _world._OnRender();
        gizmosCallback?.Invoke();
    }
    /// <summary>
    /// Internal.
    /// </summary>
    /// <param name="currentEvent"></param>
    public void _OnEvent(SDL.SDL_Event currentEvent)
    {
        this.currentEvent = currentEvent;
        if (currentEvent.type == SDL.SDL_EventType.SDL_KEYDOWN)
        {
            keys[currentEvent.key.keysym.sym] = true;
        }
        if (currentEvent.type == SDL.SDL_EventType.SDL_KEYUP)
        {
            keys[currentEvent.key.keysym.sym] = false;
        }
        _world._OnEvent();
        eventCallback?.Invoke();
    }
    /// <summary>
    /// Internal.
    /// </summary>
    public void _OnQuit()
    {
        _world._OnQuit();
        quitCallback?.Invoke();
    }
}

