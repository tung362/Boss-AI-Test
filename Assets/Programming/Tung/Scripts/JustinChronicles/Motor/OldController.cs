using UnityEngine;

[RequireComponent(typeof(OldStats))]
public class OldController : MonoBehaviour
{

    #region Variables

    #region Debug

    #if UNITY_EDITOR

    [SerializeField]
    public DebugType debug;

    protected bool debugBasic
    { get { return (debug & DebugType.Basic) == DebugType.Basic; } }
    protected bool debugIntermediate
    { get { return (debug & DebugType.Intermediate) == DebugType.Intermediate; } }
    protected bool debugAdvanced
    { get { return (debug & DebugType.Advanced) == DebugType.Advanced; } }
    protected bool debugSpam
    { get { return (debug & DebugType.Spam) == DebugType.Spam; } }
    protected bool debugVisual
    { get { return (debug & DebugType.Visual) == DebugType.Visual; } }
    protected bool debugBreak
    { get { return (debug & DebugType.Break) == DebugType.Break; } }
    protected bool debugUnstable
    { get { return (debug & DebugType.Unstable) == DebugType.Unstable; } }
    protected bool debugLogged
    { get { return (debug & DebugType.Logged) == DebugType.Logged; } }

    #endif

    #endregion

    #region Components

    // Contains information and components related to this game object.
    private OldStats _stats;
    public OldStats stats
    { get { return _stats; } }

    #endregion

    #endregion

    #region Methods

    #region Unity

    // Called on first enable, after awake.
    protected virtual void Start ()
    { Initialize(); }

    #if UNITY_EDITOR

    protected virtual void Update()
    { DebugFunc(); }

    #endif

    #endregion

    #region Commands

    public virtual void RequestMovement(Vector3 input)
    {
        Quaternion camRot = Quaternion.Euler(0, OldCameraFollow.cameraFollow.transform.rotation.eulerAngles.y, 0);
        if (input != Vector3.zero && !stats.dashing) _stats.motor.Move(camRot * input, ForceMode.VelocityChange, false);
        if (stats.attacking != true && stats.dashing != true) RequestRotation(stats.motor.rigidBody.velocity.normalized);
    }

    public virtual void RequestDash()
    { stats.Dash(); }

    public virtual void RequestAttack()
    { stats.Attack(); }

    public virtual void RequestRotation(Vector3 direction)
    {
        direction.y = 0;
        Vector3.Normalize(direction);
        if (direction != Vector3.zero) _stats.motor.Rotate(Quaternion.LookRotation(direction));
    }

    #endregion

    #region Setup

    protected virtual void Initialize()
    { _stats = GetComponent<OldStats>(); }

#if UNITY_EDITOR

    protected virtual void DebugFunc()
    { }

#endif

    #endregion

    #endregion

}
