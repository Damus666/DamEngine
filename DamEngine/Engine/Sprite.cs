using SDL2;

namespace DamEngine;

/// <summary>
/// Contains a texture ready to be rendered.
/// </summary>
public sealed class Sprite
{
    /// <summary>
    /// Sprite name.
    /// </summary>
    public string name { get; private set; }
    IntPtr _sdlTexture;
    private Window window = Engine.window;
    private Camera camera = Engine.camera;
    /// <summary>
    /// The width in pixels.
    /// </summary>
    public int width { get; private set; }
    /// <summary>
    /// The height in pixels.
    /// </summary>
    public int height { get; private set; }
    private SDL.SDL_Rect sourceRect;
    private SDL.SDL_Rect destRect = new();
    private SDL.SDL_Point center = new();

    public Sprite(string name,string imagePath)
    {
        var _sdlSurface = SDL.SDL_LoadBMP(imagePath);
        _sdlSurface = SDL.SDL_ConvertSurfaceFormat(_sdlSurface, SDL.SDL_GetWindowPixelFormat(window._sdlWindow), 0);
        _sdlTexture = SDL.SDL_CreateTextureFromSurface(window._renderer, _sdlSurface);
        SDL.SDL_Point textureSize;
        SDL.SDL_QueryTexture(_sdlTexture, out _, out _, out textureSize.x, out textureSize.y);
        width = textureSize.x;
        height = textureSize.y;
        sourceRect = new();
        sourceRect.x = 0;
        sourceRect.y = 0;
        sourceRect.w = width;
        sourceRect.h = height;
        this.name = name;
    }
    /// <summary>
    /// Internal.
    /// </summary>
    public void _Render(Entity parentEntity)
    {
        float dx = parentEntity.transform.position.x - camera.position.x;
        float dy = parentEntity.transform.position.y - camera.position.y;
        int w = (int)((width * parentEntity.transform.scale.x) * camera.zoom);
        int h = (int)((height * parentEntity.transform.scale.y) * camera.zoom);
        parentEntity.transform._lastRelativePos.x = (int)(((parentEntity.transform.position.x - camera._Left()) + ((dx * camera.zoom) - dx)) - w / 2);
        parentEntity.transform._lastRelativePos.y = (int)(((parentEntity.transform.position.y - camera._Top()) + ((dy * camera.zoom) - dy)) - h / 2);
        center.x = (int)parentEntity.transform._lastRelativePos.x + w/2;
        center.y = (int)parentEntity.transform._lastRelativePos.y + h/2;
        destRect.x = (int)parentEntity.transform._lastRelativePos.x;
        destRect.y = (int)parentEntity.transform._lastRelativePos.y;
        destRect.w = w;
        destRect.h = h;
        
        SDL.SDL_RendererFlip flipData = SDL.SDL_RendererFlip.SDL_FLIP_NONE;
        if (parentEntity.flipped[0] && parentEntity.flipped[1])
        {
            flipData = SDL.SDL_RendererFlip.SDL_FLIP_HORIZONTAL | SDL.SDL_RendererFlip.SDL_FLIP_VERTICAL;
        } else if (parentEntity.flipped[0])
        {
            flipData = SDL.SDL_RendererFlip.SDL_FLIP_HORIZONTAL;
        } else if (parentEntity.flipped[1])
        {
            flipData = SDL.SDL_RendererFlip.SDL_FLIP_VERTICAL;
        }
        
        // render
        SDL.SDL_RenderCopyEx(window._renderer, _sdlTexture, ref sourceRect, ref destRect, parentEntity.transform.rotation,ref center,flipData);
    }
    /// <summary>
    /// Internal.
    /// </summary>
    public void _FreeMemory()
    {
        SDL.SDL_DestroyTexture(_sdlTexture);
    }
}