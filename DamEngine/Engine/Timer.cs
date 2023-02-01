namespace DamEngine;

/// <summary>
/// A timer used by the <see cref="Game"/>. Don't create manually.
/// </summary>
public sealed class Timer
{
    int duration;
    Action<Timer> function;
    float startTime;
    /// <summary>
    /// Is the timer currently active?
    /// </summary>
    public bool active { get; private set; }
    private bool reactivate = false;

    public Timer(int duration, Action<Timer> function, bool startActive)
    {
        this.duration = duration;
        this.function = function;
    }
    /// <summary>
    /// Changes active to true or false.
    /// </summary>
    public void SetActive(bool status)
    {
        active = status;
        if (active)
        {
            startTime = Engine.game.ticks;
        } else
        {
            startTime = 0;
        }
    }
    /// <summary>
    /// Allows to activate a timer after the action is invoked. SetActive won't be effective.
    /// </summary>
    public void Reactivate()
    {
        reactivate = true;
    }
    /// <summary>
    /// Internal.
    /// </summary>
    public void _Update()
    {
        if (Engine.game.ticks - startTime >= duration)
        {
            if (startTime != 0)
            {
                function.Invoke(this);
            }
            SetActive(false);
            if (reactivate)
            {
                SetActive(true);
                reactivate = false;
            }
        }
    }
}
/// <summary>
/// Invoke used by entities. Don't use manually.
/// </summary>
public sealed class Invoke
{
    float durationMS;
    Entity attachedEntity;
    System.Reflection.MethodInfo method;
    float startTime = 0;

    public Invoke(float durationS, Entity attachedEntity, System.Reflection.MethodInfo method)
    {
        durationMS = durationS*1000;
        this.attachedEntity = attachedEntity;
        this.method = method;
        startTime = Engine.game.ticks;
    }

    public void _Update()
    {
        if (Engine.game.ticks - startTime >= durationMS)
        {
            method.Invoke(attachedEntity, new object[] {});
            attachedEntity.CancelInvoke(this);
        }
    }
}