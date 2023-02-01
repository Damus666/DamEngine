namespace DamEngine;
/// <summary>
/// A box bound to an entity. Don't instantiate manually.
/// </summary>
public sealed class Box
{
    private Entity parentEntity;

    public Box(Entity parentEntity)
    {
        this.parentEntity = parentEntity;
    }
    /// <returns>The width of the box.</returns>
    public float Width()
    {
        return parentEntity.sprite.width * parentEntity.transform.scale.x;
    }
    /// <returns>The height of the box.</returns>
    public float Height()
    {
        return parentEntity.sprite.height * parentEntity.transform.scale.y;
    }
    /// <returns>The center x of the box.</returns>
    public float X()
    {
        return parentEntity.transform.position.x;
    }
    /// <returns>The center y of the box.</returns>
    public float Y()
    {
        return parentEntity.transform.position.y;
    }
    /// <returns>The topleft corner of the box.</returns>
    public vec2 Topleft()
    {
        return new vec2(X() - (Width() / 2), Y() - (Height() / 2));
    }
    /// <returns>The topright corner of the box.</returns>
    public vec2 Topright()
    {
        return new vec2(X() + (Width() / 2), Y() - (Height() / 2));
    }
    /// <returns>The bottomleft corner of the box.</returns>
    public vec2 Bottomleft()
    {
        return new vec2(X() - (Width() / 2), Y() + (Height() / 2));
    }
    /// <returns>The bottomright corner of the box.</returns>
    public vec2 Bottomright()
    {
        return new vec2(X() + (Width() / 2), Y() + (Height() / 2));
    }
    /// <returns>The left x of the box.</returns>
    public float Left()
    {
        return X() - (Width() / 2);
    }
    /// <returns>The right x of the box.</returns>
    public float Right()
    {
        return X() + (Width() / 2);
    }
    /// <returns>The top y of the box.</returns>
    public float Top()
    {
        return Y() - (Height() / 2);
    }
    /// <returns>The bottom y of the box.</returns>
    public float Bottom()
    {
        return Y() + (Height() / 2);
    }
    /// <returns>The center point of the box.</returns>
    public vec2 Center()
    {
        return parentEntity.transform.position.xy;
    }
    /// <returns>The midtop point of the box.</returns>
    public vec2 Midtop()
    {
        return new vec2(X(), Y() - (Height() / 2));
    }
    /// <returns>The midbottom point of the box.</returns>
    public vec2 Midbottom()
    {
        return new vec2(X(), Y() + (Height() / 2));
    }
    /// <returns>The midleft point of the box.</returns>
    public vec2 Midleft()
    {
        return new vec2(X() - (Width() / 2), Y());
    }
    /// <returns>The midright of the box.</returns>
    public vec2 Midright()
    {
        return new vec2(X() + (Width() / 2), Y());
    }
    /// <returns>The center x position from a given right x position.</returns>
    public float XFromRight(float rightX)
    {
        return rightX - (Width() / 2);
    }
    /// <returns>The center x position from a given left x position.</returns>
    public float XFromLeft(float leftX)
    {
        return leftX + (Width() / 2);
    }
    /// <returns>The center y position from a given bottom y position.</returns>
    public float YFromBottom(float bottomY)
    {
        return bottomY - (Height() / 2);
    }
    /// <returns>The center y position from a given top y position.</returns>
    public float YFromTop(float topY)
    {
        return topY + (Height() / 2);
    }
    /// <returns>If the box is overlapping with a point.</returns>
    public bool CollidePoint(vec2 point)
    {
        return Left() <= point.x && point.x <= Right() && Top() <= point.y && point.y <= Bottom();
    }

    /// <param name="box">The box.</param>
    /// <returns>If the box is overlapping another box.</returns>
    public bool CollideBox(Box box)
    {
        if (Left() < box.Right() && Right() > box.Left() && Top() < box.Bottom() && Bottom() > box.Top())
        {
            return true;
        }
        return false;
    }
}
/// <summary>
/// Internal, used by the rigidbody.
/// </summary>
public class _OldBoundingBox
{
    vec2 _position;
    float _width;
    float _height;

    public void _Copy(Entity entity)
    {
        _position = entity.transform.position.xy;
        _width = entity.sprite.width * entity.transform.scale.x;
        _height = entity.sprite.height * entity.transform.scale.y;
    }

    public float _Left()
    {
        return _position.x - (_width / 2);
    }

    public float _Right()
    {
        return _position.x + (_width / 2);
    }

    public float _Top()
    {
        return _position.y - (_height / 2);
    }

    public float _Bottom()
    {
        return _position.y + (_height / 2);
    }
}