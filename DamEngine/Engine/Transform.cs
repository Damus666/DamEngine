namespace DamEngine;

/// <summary>
/// Defines core information for an entity.
/// </summary>
public sealed class Transform
{
    /// <summary>
    /// The world position of the entity.
    /// </summary>
    public vec2 position = vec2.Zero;
    /// <summary>
    /// The relative position to the parent.
    /// </summary>
    public vec2 localPosition = vec2.Zero;
    /// <summary>
    /// The rotation of the entity.
    /// </summary>
    public float rotation = 0;
    /// <summary>
    /// The scale of the entity
    /// </summary>
    public vec2 scale = vec2.Ones;
    /// <summary>
    /// The parent.
    /// </summary>
    public Transform? parent { get; private set; } = null;
    /// <summary>
    /// The children.
    /// </summary>
    public List<Transform> children { get; private set; } = new List<Transform>();
    /// <summary>
    /// Internal.
    /// </summary>
    public vec2 _lastRelativePos=vec2.Zero;

    public Transform(vec2 position, float rotation, vec2 scale, Transform? parent=null)
    {
        this.position = position;
        this.localPosition = position;
        this.rotation = rotation;
        this.scale = scale;
        SetParent(parent);
    }

    public Transform() { }
    /// <summary>
    /// Changes the parent.
    /// </summary>
    /// <param name="newParent"></param>
    public void SetParent(Transform? newParent)
    {
        if (parent != null)
        {
            parent.children.Remove(this);
        }
        parent = newParent;
        if (parent != null)
        {
            parent.children.Add(this);
        }
    }
    /// <summary>
    /// Internal.
    /// </summary>
    public void _Update()
    {
        foreach (Transform child in children)
        {
            child.position = position + child.localPosition;
        }
    }
}