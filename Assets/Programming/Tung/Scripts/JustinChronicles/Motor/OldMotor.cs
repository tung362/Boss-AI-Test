using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class OldMotor : MonoBehaviour
{

    #region Variables

    #region Debug

    #if UNITY_EDITOR

    [SerializeField]
    public OldDebugType debug;

    protected bool debugBasic
    { get { return (debug & OldDebugType.Basic) == OldDebugType.Basic; } }
    protected bool debugIntermediate
    { get { return (debug & OldDebugType.Intermediate) == OldDebugType.Intermediate; } }
    protected bool debugAdvanced
    { get { return (debug & OldDebugType.Advanced) == OldDebugType.Advanced; } }
    protected bool debugSpam
    { get { return (debug & OldDebugType.Spam) == OldDebugType.Spam; } }
    protected bool debugVisual
    { get { return (debug & OldDebugType.Visual) == OldDebugType.Visual; } }
    protected bool debugBreak
    { get { return (debug & OldDebugType.Break) == OldDebugType.Break; } }
    protected bool debugUnstable
    { get { return (debug & OldDebugType.Unstable) == OldDebugType.Unstable; } }
    protected bool debugLogged
    { get { return (debug & OldDebugType.Logged) == OldDebugType.Logged; } }

    #endif

    #endregion

    #region Modifiers

    #region Speed

    // Acceleration modifies how fast we ramp up velocity.
    public float acceleration;

    // Soft velocity cap is where we stop accelerating.
    public float softVelocityCap;

    // Hard velocity cap is where we clamp velocity.
    public float hardVelocityCap;

    public float rotationSpeed;

    #endregion

    #endregion

    #region Components

    private Rigidbody _rigidBody;
    public  Rigidbody  rigidBody
    { get { return _rigidBody; } }

    #endregion

    #endregion

    #region Methods

    #region Unity

    // Called on first enable, after awake.
    protected virtual void Start ()
    { Initialize(); }

    // Called every physics update.
    protected virtual void FixedUpdate()
    { ApplyHardVelocityCap(); }

    #if UNITY_EDITOR

    protected virtual void Update()
    { DebugFunc(); }

    #endif

    #endregion

    #region PublicControls

    // Used to move the object with physics.
    public virtual void Move(Vector3 input, ForceMode mode, bool relative)
    {
        ApplySoftVelocityCap(ref input);
        if (relative) _rigidBody.AddRelativeForce(input * acceleration, mode);
        else _rigidBody.AddForce(input * acceleration, mode);
    }

    public virtual void Rotate(Quaternion rotation)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);
    }

    // Used to directly set an objects velocity.
    public virtual void SetVelocity(Vector3 input)
    { rigidBody.velocity = input; ApplyHardVelocityCap(); }

    #endregion

    #region Velocity

    // Used to cap acceleration based on velocity.
    protected virtual void ApplySoftVelocityCap(ref Vector3 acc)
    { acc = Vector3.ClampMagnitude(acc, Mathf.Clamp(softVelocityCap - rigidBody.velocity.magnitude, 0, hardVelocityCap)); }

    // Used to cap velocity.
    protected virtual void ApplyHardVelocityCap()
    { rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, hardVelocityCap); }

    #endregion

    #region Setup

    // Used for initial setup.
    protected virtual void Initialize()
    {
        _rigidBody = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.identity;
    }

#if UNITY_EDITOR

    protected virtual void DebugFunc()
    {
        // Velocity print
        if (debugBasic && debugSpam) Debug.Log(name + "'s Vel: " + rigidBody.velocity.ToString() + "; ");
    }

#endif

    #endregion

    #endregion

}
