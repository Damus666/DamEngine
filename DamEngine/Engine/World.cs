using System.Collections.Generic;

namespace DamEngine;

/// <summary>
/// Contains all the entities and more.
/// </summary>
public sealed class World
{
    private List<Entity> entities = new();
    /// <summary>
    /// Internal.
    /// </summary>
    public List<Entity> _entities { get { return entities; } }
    private List<Entity> entitiesForRendering = new();
    private List<Rigidbody> rigidbodies = new();
    /// <summary>
    /// Internal
    /// </summary>
    public List<Rigidbody> _rigidbodies { get { return rigidbodies; } }
    /// <summary>
    /// The global gravity scale used by rigidbodies.
    /// </summary>
    public float gravityScale=9.81f;
    /// <summary>
    /// Internal.
    /// </summary>
    public int _bodyID { get; private set; } = 0;
    /// <summary>
    /// Whether OnAwake has been called yet.
    /// </summary>
    public bool hasStarted { get; private set; } = false;

    /// <summary>
    /// Changes the global gravity scale.
    /// </summary>
    /// <param name="gravityScale">The new value.</param>
    public void Setup(float gravityScale = 9.81f)
    {
        this.gravityScale = gravityScale;
    }
    /// <summary>
    /// Returns the first entity with a name, if any.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The entity if found.</returns>
    public Entity? Find(string name)
    {
        foreach (Entity entity in entities)
        {
            if (entity.name == name)
            {
                return entity;
            }
        }
        return null;
    }
    /// <summary>
    /// Returns all the entities with a tag.
    /// </summary>
    /// <param name="tag">The tag.</param>
    /// <returns>The list with entities.</returns>
    public List<Entity> EntitiesOfTag(string tag)
    {
        List<Entity> found = new List<Entity>();
        foreach (Entity entity in entities)
        {
            if (entity.tags.Contains(tag))
            {
                found.Add(entity);
            }
        }
        return found;
    }
    /// <summary>
    /// Register an entity to the world. If the game has started OnAwake is called.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>The entity (to perform other actions on it).</returns>
    public Entity Spawn(Entity entity)
    {
        entities.Add(entity);
        entitiesForRendering.Add(entity);
        SortSprites();
        if (hasStarted)
        {
            entity.OnAwake();
        }
        return entity;
    }
    /// <summary>
    /// Removes an entity from the world. OnDestroy is called.
    /// </summary>
    /// <param name="entity">The entity.</param>
    public void Destroy(Entity entity)
    {
        if (entities.Contains(entity))
        {
            entity.OnDestroy();
            entities.Remove(entity);
            entitiesForRendering.Remove(entity);
            if (entity.rigidbody != null && rigidbodies.Contains(entity.rigidbody))
            {
                rigidbodies.Remove(entity.rigidbody);
            }
            SortSprites();
        }
    }
    /// <summary>
    /// Internal.
    /// </summary>
    public void _OnUpdate()
    {
        foreach (Entity entity in entities)
        {
            if (entity.active)
            {
                if (entity._invokes.Count > 0)
                {
                    List<Invoke> tempInvokes = new List<Invoke>(entity._invokes);
                    foreach (Invoke i in tempInvokes)
                    {
                        i._Update();
                    }
                }
                entity.OnUpdate();
                if (entity.rigidbody != null)
                {
                    entity.rigidbody._Update();
                }
            }
            entity.transform._Update();
        }
    }
    /// <summary>
    /// Internal.
    /// </summary>
    public void _OnRender()
    {
        foreach (Entity e in entitiesForRendering)
        {
            if (e.visible)
            {
                e.sprite._Render(e);
                e.OnGizmos();
            }
        }
    }
    /// <summary>
    /// Internal.
    /// </summary>
    public void _OnEvent()
    {
        foreach (Entity entity in entities)
        {
            if (entity.active)
            {
                entity.OnEvent();
            }
        }
    }
    /// <summary>
    /// Internal.
    /// </summary>
    public void _OnQuit()
    {
        foreach (Entity entity in entities)
        {
            entity.OnDestroy();
            entity.sprite._FreeMemory();
        }
        entities.Clear();
    }
    /// <summary>
    /// Internal.
    /// </summary>
    public void _OnAwake()
    {
        hasStarted = true;
        foreach (Entity entity in entities)
        {
            entity.OnAwake();
        }
    }
    /// <summary>
    /// Internal.
    /// </summary>
    /// <param name="body"></param>
    public void _RegisterRigidbody(Rigidbody body)
    {
        rigidbodies.Add(body);
        _bodyID++;
    }
    /// <summary>
    /// Internal.
    /// </summary>
    /// <param name="body"></param>
    public void _DestroyRigidbody(Rigidbody body)
    {
        if (rigidbodies.Contains(body))
        {
            rigidbodies.Remove(body);
        }
    }

    private void SortSprites()
    {
        entitiesForRendering = entitiesForRendering.OrderBy(x => x.renderingOrder).ToList();
    }
}