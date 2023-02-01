using SDL2;

namespace DamEngine;

/// <summary>
/// Constains the instance of all the sprites registered.
/// </summary>
public sealed class SpriteManager
{
    /// <summary>
    /// The List<> of the sprites.
    /// </summary>
    public List<Sprite> sprites { get; private set; } = new();

    /// <summary>
    /// Register a sprite to the list.
    /// </summary>
    /// <param name="sprite">The sprite to register.</param>
    /// <returns>The sprite registered.</returns>
    public Sprite Register(Sprite sprite)
    {
        sprites.Add(sprite);
        return sprite;
    }

    /// <summary>
    /// Returns the sprite with a give name, if it exists.
    /// </summary>
    /// <param name="spriteName">The name of the sprite.</param>
    /// <returns>The sprite if found, otherwise null.</returns>
    public Sprite? Get(string spriteName)
    {
        foreach (Sprite sprite in sprites)
        {
            if (sprite.name == spriteName)
            {
                return sprite;
            }
        }
        return null;
    }

    /// <summary>
    /// Internal method, called on application quitting.
    /// </summary>
    public void _OnQuit()
    {
        foreach (Sprite sprite in sprites)
        {
            sprite._FreeMemory();
        }
    }
}