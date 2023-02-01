namespace DamEngine;

/// <summary>
/// All entities inherit from this one.
/// </summary>
public class Entity
{
    /// <summary>
    /// Defines position, rotation and scale.
    /// </summary>
    public Transform transform;
    /// <summary>
    /// The instance of the sprite.
    /// </summary>
    public Sprite sprite { get; private set; }
    /// <summary>
    /// The box around the entity.
    /// </summary>
    public Box boundingBox { get; private set; }
    /// <summary>
    /// The rigidbody if added.
    /// </summary>
    public Rigidbody? rigidbody { get; private set; }
    /// <summary>
    /// The name.
    /// </summary>
    public string name = "entity";
    /// <summary>
    /// All the tags.
    /// </summary>
    public List<string> tags = new();
    /// <summary>
    /// If false it won't be visible and OnGizmos won't be called.
    /// </summary>
    public bool visible = true;
    /// <summary>
    /// If false OnUpdate and OnEvent won't be called. Collisions won't be detected.
    /// </summary>
    public bool active = true;
    /// <summary>
    /// Order for rendering the sprite.
    /// </summary>
    public int renderingOrder { get; private set; } = 0;
    /// <summary>
    /// The flip status.
    /// </summary>
    public List<bool> flipped = new List<bool> { false, false };
    /// <summary>
    /// The list of invokes in action. Internal.
    /// </summary>
    public List<Invoke> _invokes { get; private set; } = new();

    /// <summary>
    /// Setups the fields
    /// </summary>
    /// <exception cref="NullReferenceException">If the sprite doesn't exist.</exception>
    public Entity(Transform transform, string spriteName, string name="entity", List<string>? tags=null, bool visible=true, bool active=true, int renderingOrder=0)
    {
        this.transform = transform;
        this.renderingOrder = renderingOrder;
        sprite = Engine.spriteManager.Get(spriteName);
        if (sprite == null)
        {
            throw new NullReferenceException($"Sprite manager does not contain a sprite named '{spriteName}'");
        }
        this.name = name;
        if (tags != null)
        {
            this.tags = tags;
        }
        this.visible = visible;
        this.active = active;
        boundingBox = new(this);
    }
    /// <summary>
    /// Adds a rigidbody to the entity.
    /// </summary>
    /// <param name="bodyType">Either static or dynamic.</param>
    /// <param name="mass">The mass.</param>
    /// <param name="useGravity">Should the body be affected by gravity</param>
    /// <param name="isTrigger">Collisions will still be detected.</param>
    /// <param name="material"><see cref="PhysicMaterial"/> defining friction and reaction force.</param>
    /// <returns></returns>
    public Rigidbody AddRigidbody(BodyType bodyType = BodyType.Dynamic,float mass = 1,bool useGravity = true,bool isTrigger=false,PhysicMaterial? material = null)
    {
        if (material == null)
        {
            material = new PhysicMaterial();
        }
        rigidbody = new(bodyType, this, mass, material, useGravity, isTrigger, Engine.world._bodyID);
        Engine.world._RegisterRigidbody(rigidbody);
        return rigidbody;
    }
    /// <summary>
    /// Removes the added rigidbody.
    /// </summary>
    public void RemoveRigidbody()
    {
        if (rigidbody != null)
        {
            Engine.world._DestroyRigidbody(rigidbody);
            rigidbody = null;
        }
    }
    /// <summary>
    /// Changes the flip status.
    /// </summary>
    /// <param name="x">Flipped on the x.</param>
    /// <param name="y">Flipped on the y.</param>
    public void Flip(bool x, bool y)
    {
        flipped[0] = x;
        flipped[1] = y;
    }
    /// <summary>
    /// Invokes a class method after some time.
    /// </summary>
    /// <param name="methodName">The name.</param>
    /// <param name="timeS">The time in seconds.</param>
    /// <returns>The created <see cref="DamEngine.Invoke"/></returns>
    /// <exception cref="NullReferenceException">If the methods doesn't exist.</exception>
    public Invoke Invoke(string methodName,float timeS)
    {
        var method = GetType().GetMethod(methodName);
        if (method == null)
        {
            throw new NullReferenceException($"Method '{methodName}' was not found");
        }
        Invoke i = new(timeS, this, method);
        _invokes.Add(i);
        return i;
    }
    /// <summary>
    /// Cancel a created invoke.
    /// </summary>
    /// <param name="invoke">The invoke.</param>
    /// <returns>true if cancelled.</returns>
    public bool CancelInvoke(Invoke invoke)
    {
        if (_invokes.Contains(invoke))
        {
            _invokes.Remove(invoke);
            return true;
        }
        return false;
    }
    /// <summary>
    /// Called every frame. Should be overrided.
    /// </summary>
    public virtual void OnUpdate()
    {

    }
    /// <summary>
    /// Called after rendering. Should be overrided.
    /// </summary>
    public virtual void OnGizmos()
    {

    }
    /// <summary>
    /// Called for every event. Should be overrided.
    /// </summary>
    public virtual void OnEvent()
    {

    }
    /// <summary>
    /// Called on game staring/when spawning. Should be overrided.
    /// </summary>
    public virtual void OnAwake()
    {

    }
    /// <summary>
    /// Called after <see cref="World.Destroy"/>. Should be overrided.
    /// </summary>
    public virtual void OnDestroy()
    {

    }
    /// <summary>
    /// Called at the first collision with a rigidbody. Should be overrided.
    /// </summary>
    public virtual void OnCollisionEnter(Rigidbody rigidbody, vec2 direction)
    {

    }
    /// <summary>
    /// Called at the last collision with a rigidbody. Should be overrided.
    /// </summary>
    public virtual void OnCollisionExit(Rigidbody rigidbody)
    {

    }
    /// <summary>
    /// Called after the first collision with a rigidbody for every new collision. Should be overrided.
    /// </summary>
    public virtual void OnCollisionStay(Rigidbody rigidbody)
    {

    }
}
