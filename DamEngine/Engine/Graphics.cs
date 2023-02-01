using SDL2;

namespace DamEngine;
/// <summary>
/// Contains useful rendering methods.
/// </summary>
public static class Graphics
{
    static IntPtr renderer = Engine.window._renderer;
    static Camera camera = Engine.camera;
    /// <summary>
    /// Renders a line given 2 points and r,g,b color values.
    /// </summary>
    public static void RenderLine(vec2 point1,vec2 point2,int r, int g, int b)
    {
        SDL.SDL_SetRenderDrawColor(renderer, (byte)r, (byte)g, (byte)b, 255);
        SDL.SDL_RenderDrawLine(renderer,(int)point1.x, (int)point1.y, (int)point2.x, (int)point2.y);
    }
    /// <summary>
    /// Renders multiple lines given a list of points and r,g,b color values.
    /// </summary>
    public static void RenderLines(List<vec2> points,int r, int g, int b,bool connectFirstLast=false)
    {
        SDL.SDL_SetRenderDrawColor(renderer, (byte)r, (byte)g, (byte)b, 255);
        int i = 0;
        foreach (vec2 point in points)
        {
            if (points.Count > i + 1)
            {
                SDL.SDL_RenderDrawLine(renderer, (int)point.x, (int)point.y, (int)points[i+1].x, (int)points[i + 1].y);
            } else
            {
                if (connectFirstLast)
                {
                    SDL.SDL_RenderDrawLine(renderer, (int)point.x, (int)point.y, (int)points[0].x, (int)points[0].y);
                }
            }
            i++;
        }
    }
    /// <summary>
    /// Renders the outline of a rect given r,g,b color values.
    /// </summary>
    public static void RenderRectOutline(int x, int y, int w, int h,int r, int g, int b)
    {
        SDL.SDL_SetRenderDrawColor(renderer, (byte)r, (byte)g, (byte)b, 255);
        SDL.SDL_Rect rect;
        rect.x = x;
        rect.y = y;
        rect.w = w;
        rect.h = h;
        SDL.SDL_RenderDrawRect(renderer, ref rect);
    }
    /// <summary>
    /// Renders a rect given r,g,b color values.
    /// </summary>
    public static void RenderRectFill(int x, int y, int w, int h, int r, int g, int b)
    {
        SDL.SDL_SetRenderDrawColor(renderer, (byte)r, (byte)g, (byte)b, 255);
        SDL.SDL_Rect rect;
        rect.x = x;
        rect.y = y;
        rect.w = w;
        rect.h = h;
        SDL.SDL_RenderFillRect(renderer, ref rect);
    }
    /// <summary>
    /// Renders the outline of a box given r,g,b color values. Can be inflated.
    /// </summary>
    public static void RenderBoxOutline(Entity parentOfBox,int r, int g, int b,vec2? inflate = null)
    {
        vec2 inf;
        if (inflate == null)
        {
            inf = UnitVectors.Zero;
        }
        else { inf = (vec2)inflate; }
        inf *= camera.zoom;
        RenderRectOutline((int)(parentOfBox.transform._lastRelativePos.x - inf.x / 2),
            (int)(parentOfBox.transform._lastRelativePos.y - inf.y / 2),
            (int)(parentOfBox.boundingBox.Width()*camera.zoom + inf.x),
            (int)(parentOfBox.boundingBox.Height()*camera.zoom + inf.y), r, g, b);
    }
    /// <summary>
    /// Renders a box given r,g,b color values. Can be inflated.
    /// </summary>
    public static void RenderBoxFill(Entity parentOfBox, int r, int g, int b, vec2? inflate = null)
    {
        vec2 inf;
        if (inflate == null)
        {
            inf = UnitVectors.Zero;
        } else { inf = (vec2)inflate; }
        inf *= camera.zoom;
        RenderRectFill((int)(parentOfBox.transform._lastRelativePos.x-inf.x/2),
            (int)(parentOfBox.transform._lastRelativePos.y-inf.y/2),
            (int)(parentOfBox.boundingBox.Width()*camera.zoom+inf.x),
            (int)(parentOfBox.boundingBox.Height()*camera.zoom+inf.y), r, g, b);
    }
    /// <summary>
    /// Renders a ray given origin, direction, lenght and r,g,b color values. Can be inflated.
    /// </summary>
    public static void RenderRay(vec2 origin, vec2 direction, float length, int r, int g, int b)
    {
        vec2 o = camera.WorldToScreen(origin);
        vec2 p2 = o + (direction * (length * camera.zoom));
        RenderLine(o, p2, r, g, b);
    }
}