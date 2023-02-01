using SDL2;

namespace DamEngine;

/// <summary>
/// Camera class. All entites are rendered relative to it.
/// </summary>
public sealed class Camera
{
    /// <summary>
    /// The current camera position.
    /// </summary>
    public vec2 position = new(0,0);
    /// <summary>
    /// How much is the camera zoomed.
    /// </summary>
    public float zoom { get; private set; } = 1;
    /// <summary>
    /// The zoom won't be lower than this.
    /// </summary>
    public float minZoom { get; private set; } = 0.05f;
    /// <summary>
    /// The zoom won't be bigger than this.
    /// </summary>
    public float maxZoom { get; private set; } = 10;
    Window window = Engine.window;

    /// <summary>
    /// Setup the camera attributes.
    /// </summary>
    /// <param name="position">Starting position.</param>
    /// <param name="zoom">Starting zoom.</param>
    /// <param name="minZoom">Lower zoom.</param>
    /// <param name="maxZoom">Highest zoom.</param>
    public void Setup(vec2 position,float zoom = 1,float minZoom = 0.05f,float maxZoom = 10)
    {
        this.position = position;
        this.minZoom = minZoom;
        this.maxZoom = maxZoom;
        SetZoom(zoom);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>The left x side of the camera.</returns>
    public float Left()
    {
        return position.x - ((window.width / zoom) / 2);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>The right x side of the camera.</returns>
    public float Right()
    {
        return position.x + ((window.width / zoom) / 2);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>The top y side of the camera.</returns>
    public float Top()
    {
        return position.y - ((window.height / zoom) / 2);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>The bottom y side of the camera.</returns>
    public float Bottom()
    {
        return position.y + ((window.height / zoom) / 2);
    }
    /// <summary>
    /// Internal method.
    /// </summary>
    /// <returns></returns>
    public float _Left()
    {
        return position.x - (window.width / 2);
    }
    /// <summary>
    /// Internal method.
    /// </summary>
    /// <returns></returns>
    public float _Top()
    {
        return position.y - (window.height / 2);
    }
    /// <summary>
    /// Sets the new zoom. Clamped between min and max.
    /// </summary>
    /// <param name="zoom">The new zoom value.</param>
    public void SetZoom(float zoom)
    {
        this.zoom = Math.Clamp(zoom, minZoom, maxZoom);
    }
    /// <summary>
    /// Changes the zoom of an amount. Clamped between min and max.
    /// </summary>
    /// <param name="amount"></param>
    public void Zoom(float amount)
    {
        zoom += amount;
        zoom = Math.Clamp(zoom, minZoom, maxZoom);
    }
    /// <summary>
    /// Changes the camera position.
    /// </summary>
    /// <param name="amountX"></param>
    /// <param name="amountY"></param>
    public void Move(float amountX, float amountY)
    {
        position.x += amountX;
        position.y += amountY;
    }
    /// <summary>
    /// Converts a screen point to a world point.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <returns>Converted point.</returns>
    public vec2 ScreenToWorld(vec2 point)
    {
        return new vec2(((point.x / zoom) + Left()), ((point.y / zoom) + Top()));
    }
    /// <summary>
    /// Converts a world point to a screen point.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <returns>Converted point.</returns>
    public vec2 WorldToScreen(vec2 point)
    {
        return new vec2((point.x - Left())*zoom, (point.y - Top())*zoom);
    }
    /// <summary>
    /// Shorthand for zooming the camera with the mouse wheel.
    /// </summary>
    /// <param name="zoomSpeed">Speed of zoom.</param>
    public void ZoomEvent(float zoomSpeed = 0.08f)
    {
        SDL.SDL_Event e = Engine.game.currentEvent;
        if (e.type == SDL.SDL_EventType.SDL_MOUSEWHEEL)
        {
            Zoom(e.wheel.y * zoomSpeed);
        }
    }
    /// <summary>
    /// Shorthand for moving the camera by dragging the mouse.
    /// </summary>
    /// <param name="dragButton">What button will make the camera move on press.s</param>
    public void DragEvent(int dragButton = 2)
    {
        SDL.SDL_Event e = Engine.game.currentEvent;
        if (e.type == SDL.SDL_EventType.SDL_MOUSEMOTION)
        {
            if (SDL.SDL_GetMouseState(out _, out _) == dragButton)
            {
                Move(-e.motion.xrel/zoom,-e.motion.yrel/zoom);
            }
        }
    }

}