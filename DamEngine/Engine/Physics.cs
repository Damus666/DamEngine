namespace DamEngine;
/// <summary>
/// Contains useful physics related methods.
/// </summary>
public static class Physics
{
    /// <summary>
    /// Returns all the boxes colliding with a box.
    /// </summary>
    /// <param name="box">The box.</param>
    /// <param name="targetTag">The optional tag to filter.</param>
    /// <returns>List of <see cref="Box"/></returns>
    public static List<Box> BoxCast(Box box, string? targetTag = null)
    {
        List<Box> boxes = new();
        foreach (Entity b in Engine.world._entities)
        {
            if (b.active)
            {
                if (targetTag == null || b.tags.Contains(targetTag))
                {
                    if (box.CollideBox(b.boundingBox))
                    {
                        if (b.boundingBox != box)
                        {
                            boxes.Add(b.boundingBox);
                        }
                    }
                }
            }
        }
        return boxes;
    }
    /// <returns>The intersection point of 2 lines if any.</returns>
    public static vec2? LineLineIntersection(vec2 point1, vec2 point2, vec2 point3, vec2 point4)
    {
        float d = ((point4.y - point3.y) * (point2.x - point1.x) - (point4.x - point3.x) * (point2.y - point1.y));
        if (d == 0)
        {
            return null;
        }
        float ua = ((point4.x - point3.x) * (point1.y - point3.y) - (point4.y - point3.y) * (point1.x - point3.x)) / d;
        float ub = ((point2.x - point1.x) * (point1.y - point3.y) - (point2.y - point1.y) * (point1.x - point3.x)) / d;
        if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
        {
            return new vec2(point1.x + (ua * (point2.x - point1.x)), point1.y + (ua * (point2.y - point1.y)));
        }
        return null;
    }
    /// <returns>If a line and a box are cooliding.</returns>
    public static bool LineBoxIntersection(Box box, vec2 point1, vec2 point2)
    {
        vec2? left = LineLineIntersection(point1, point2, box.Topleft(), box.Bottomleft());
        vec2? right = LineLineIntersection(point1, point2, box.Topright(), box.Bottomright());
        vec2? top = LineLineIntersection(point1, point2, box.Topleft(), box.Topright());
        vec2? bottom = LineLineIntersection(point1, point2, box.Bottomleft(), box.Bottomright());
        if (left != null || right != null || top != null || bottom != null)
        {
            return true;
        }
        return false;
    }
    /// <returns>If 2 rects are intersecting.</returns>
    public static bool RectRectIntersection(float x1, float x2, float y1, float y2, float w1, float w2, float h1, float h2)
    {
        if (x1<x2+w2 && x1+w1 > x2 && y1 < y2+h2 && y1+h1 > y2)
        {
            return true;
        }
        return false;
    }
    /// <returns>If 2 rects are intersecting.</returns>
    public static bool RectRectIntersection(int x1, int x2, int y1, int y2, int w1, int w2, int h1, int h2)
    {
        if (x1 < x2 + w2 && x1 + w1 > x2 && y1 < y2 + h2 && y1 + h1 > y2)
        {
            return true;
        }
        return false;
    }
    /// <returns>Returns the list of <see cref="Entity"/> colliding with a ray.</returns>
    public static List<Entity> Raycast(vec2 origin, vec2 direction, float length, string? filterTag = null)
    {
        List<Entity> found = new();
        vec2 p2 = origin + (direction * length);
        foreach (Entity e in Engine.world._entities)
        {
            if (e.active)
            {
                if (filterTag == null ||e.tags.Contains(filterTag))
                {
                    if (LineBoxIntersection(e.boundingBox, origin, p2))
                    {
                        found.Add(e);
                    }
                }
            }
        }
        return found;
    }
}