namespace DamEngine;

/// <summary>
/// Static or dynamic. Used by the rigidbody.
/// </summary>
public enum BodyType
{
    Static,Dynamic
}
/// <summary>
/// Defines friction and reaction factor for a rigidbody.
/// </summary>
public sealed class PhysicMaterial
{
    public float friction = 0.002f;
    public float reactionFactor = 0.15f;

    public PhysicMaterial(float friction = 0.001f, float reactionFactor=0.15f)
    {
        this.friction = friction;
        this.reactionFactor = reactionFactor;
    }
}
/// <summary>
/// Makes collisions happen between entities.
/// </summary>
public sealed class Rigidbody
{
    /// <summary>
    /// The type of the body.
    /// </summary>
    BodyType bodyType = BodyType.Dynamic;
    /// <summary>
    /// The entity attached to this body.
    /// </summary>
    public Entity parentEntity { get; private set; }
    /// <summary>
    /// The mass.
    /// </summary>
    public float mass;
    /// <summary>
    /// The <see cref="PhysicMaterial"/>
    /// </summary>
    public PhysicMaterial physicMaterial ;
    /// <summary>
    /// Whether gravity affects this body.
    /// </summary>
    public bool useGravity=true;
    /// <summary>
    /// Current body acceleration.
    /// </summary>
    public vec2 acceleration = new vec2();
    /// <summary>
    /// Current velocity of the body.
    /// </summary>
    public vec2 velocity = new vec2();
    /// <summary>
    /// Internal.
    /// </summary>
    public _OldBoundingBox _oldBox { get; private set; }
    /// <summary>
    /// If true collisions won't be visible, only detected.
    /// </summary>
    public bool isTrigger=false;
    /// <summary>
    /// Internal.
    /// </summary>
    public int _id { get; private set; }
    Dictionary<int, bool> isCoolidingWith = new();
    /// <summary>
    /// Should the body check collisions.
    /// </summary>
    public bool enabled = true;

    public Rigidbody(BodyType bodyType, Entity parentEntity, float mass, PhysicMaterial physicMaterial, bool useGravity, bool isTrigger, int id)
    {
        this.bodyType = bodyType;
        this.parentEntity = parentEntity;
        this.mass = mass;
        this.physicMaterial = physicMaterial;
        this.useGravity = useGravity;
        this.isTrigger = isTrigger;
        _oldBox = new _OldBoundingBox();
        _oldBox._Copy(parentEntity);
    }
    /// <summary>
    /// Adds an acceleration to the body.
    /// </summary>
    /// <param name="acceleration"></param>
    public void Accelerate(vec2 acceleration)
    {
        this.acceleration += acceleration;
    }
    /// <summary>
    /// Applies a force to the body.
    /// </summary>
    /// <param name="force"></param>
    public void AddForce(vec2 force)
    {
        this.acceleration += force / mass;
    }
    /// <summary>
    /// Internal.
    /// </summary>
    public void _Update()
    {
        if (enabled)
        {
            if (bodyType == BodyType.Dynamic && useGravity)
            {
                Accelerate(new vec2(0, Engine.world.gravityScale));
            }
            velocity += acceleration;
            parentEntity.transform.position += velocity * Engine.game.deltaTime;
            acceleration.x = 0;
            acceleration.y = 0;

            // COLLISION START
            if (bodyType == BodyType.Dynamic)
            {
                foreach (Rigidbody body in Engine.world._rigidbodies)
                {
                    if (body != this && body.parentEntity.active)
                    {
                        if (!isCoolidingWith.ContainsKey(body._id))
                        {
                            isCoolidingWith[body._id] = false;
                        }
                        if (parentEntity.boundingBox.CollideBox(body.parentEntity.boundingBox))
                        {
                            Box myb = parentEntity.boundingBox;
                            Box hisb = body.parentEntity.boundingBox;
                            _OldBoundingBox myob = _oldBox;
                            _OldBoundingBox hisob = body._oldBox;
                            Transform myt = parentEntity.transform;
                            vec2 collisionDir = new();
                            // RIGHT
                            if (myb.Right() >= hisb.Left() && myob._Right() <= hisob._Left())
                            {
                                collisionDir.x = 1;
                                if (!body.isTrigger)
                                {
                                    myt.position.x = myb.XFromRight(hisb.Left());
                                    float prev = velocity.x;
                                    velocity.x = 0;
                                    acceleration.x = 0;
                                    Accelerate(new vec2(
                                        (-prev * body.physicMaterial.reactionFactor) / mass,
                                        -velocity.y * (physicMaterial.friction * mass)
                                        ));
                                }
                            }
                            // LEFT
                            if (myb.Left() <= hisb.Right() && myob._Left() >= hisob._Right())
                            {
                                collisionDir.x = -1;
                                if (!body.isTrigger)
                                {
                                    myt.position.x = myb.XFromLeft(hisb.Right());
                                    float prev = velocity.x;
                                    velocity.x = 0;
                                    acceleration.x = 0;
                                    Accelerate(new vec2(
                                        (-prev * body.physicMaterial.reactionFactor) / mass,
                                        -velocity.y * (physicMaterial.friction * mass)
                                        ));
                                }
                            }
                            // BOTTOM
                            if (myb.Bottom() >= hisb.Top() && myob._Bottom() <= hisob._Top())
                            {
                                collisionDir.y = -1;
                                if (!body.isTrigger)
                                {
                                    myt.position.y = myb.YFromBottom(hisb.Top());
                                    float prev = velocity.y;
                                    velocity.y = 0;
                                    acceleration.y = 0;
                                    Accelerate(new vec2(
                                        -velocity.x * (physicMaterial.friction * mass),
                                        (-prev * body.physicMaterial.reactionFactor) / mass
                                        ));
                                }
                            }
                            // TOP
                            if (myb.Top() <= hisb.Bottom() && myob._Top() >= hisob._Bottom())
                            {
                                collisionDir.y = 1;
                                if (!body.isTrigger)
                                {
                                    myt.position.y = myb.YFromTop(hisb.Bottom());
                                    float prev = velocity.y;
                                    velocity.y = 0;
                                    acceleration.y = 0;
                                    Accelerate(new vec2(
                                        -velocity.x * (physicMaterial.friction * mass),
                                        (-prev * body.physicMaterial.reactionFactor) / mass
                                        ));
                                }
                            }
                            //FINAL
                            if (isCoolidingWith[body._id])
                            {
                                parentEntity.OnCollisionStay(body);
                                body.parentEntity.OnCollisionStay(this);
                            }
                            else
                            {
                                isCoolidingWith[body._id] = true;
                                parentEntity.OnCollisionEnter(body, collisionDir);
                                body.parentEntity.OnCollisionEnter(this, -collisionDir);
                            }
                        }
                        else
                        {
                            if (isCoolidingWith[body._id])
                            {
                                isCoolidingWith[body._id] = false;
                                parentEntity.OnCollisionExit(body);
                                body.parentEntity.OnCollisionExit(this);
                            }
                        }
                    }
                }
            }
            // COLLISION END

            _oldBox._Copy(parentEntity);
        }
    }
}