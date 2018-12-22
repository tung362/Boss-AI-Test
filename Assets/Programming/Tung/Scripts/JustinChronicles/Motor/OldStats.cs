using UnityEngine;

public class OldStats : MonoBehaviour
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

    // Used for physics based interaction.
    private OldMotor _motor;
    public  OldMotor  motor
    { get { return _motor; } }

    private Animator _animator;
    public Animator animator
    { get { return _animator; } }

    OldNonPlayerInput aiInput;

    #endregion

    public float hitInvulnerabilityLength;
    public float invulnTimer;
    public bool invuln;

    public int maxHealth;
    public int health;

    public bool attacking;
    public float attackLength;
    float attackTimer;
    public float attackCooldownLength;
    float attackCooldownTimer;

    public bool dashing;
    public float dashLength;
    float dashTimer;
    public float dashCooldownLength;
    float dashCooldownTimer;
    public float dashSpeed;

    public bool AI
    { get { return aiInput != null; } }

    #region Properties

    #region ComponentCheck

    // Used in quickly checking if a component is present.

    public bool hasMotor
    { get { return _motor != null; } }

    #endregion

    #endregion

    #endregion

    #region Methods

    #region Unity

    // Called on first enable, after awake.
    protected virtual void Start ()
    { NetworkCheck(); Initialize(); }

    protected virtual void Update()
    { InvulnStep(); SetAnimations(); AttackStep(); DashStep(); }

#if UNITY_EDITOR

    //protected virtual void Update()
    //{ DebugFunc(); }

#endif

    #endregion

    #region Combat

    public void Attack()
    { if (!attacking && attackCooldownTimer >= attackCooldownLength) { attacking = true; attackCooldownTimer = 0; } }

    void AttackStep()
    {
        if (attacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackLength)
            {
                attacking = false;
                attackTimer = 0;
            }
        }
        else attackCooldownTimer += Time.deltaTime;
    }

    public void Dash()
    { if (!dashing && dashCooldownTimer >= dashCooldownLength) { dashing = true; dashCooldownTimer = 0; } }

    void DashStep()
    {
        if (dashing)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= dashLength)
            {
                dashing = false;
                dashTimer = 0;
                motor.SetVelocity(transform.forward * dashSpeed);
            }
        }
        else dashCooldownTimer += Time.deltaTime;
    }

    void OnTakeDamage(int hp)
    {
        invuln = true;
        invulnTimer = 0;
    }

    void InvulnStep()
    {
        if (invuln)
        {
            invulnTimer += Time.deltaTime;
            if (invulnTimer >= hitInvulnerabilityLength) invuln = false;
        }
    }

    #endregion

    #region Animations

    protected virtual void SetAnimations()
    { }

    #endregion

    #region Setup

    protected virtual void NetworkCheck()
    { }

    // Used for initial setup.
    protected virtual void Initialize()
    { aiInput = GetComponent<OldNonPlayerInput>(); health = maxHealth; _motor = GetComponent<OldMotor>(); if ((_animator = GetComponent<Animator>()) == null || _animator.enabled == false) _animator = transform.GetChild(0).GetComponent<Animator>(); if (!AI) OldCameraFollow.cameraFollow.SetFollowTarget(transform); }

#if UNITY_EDITOR

    protected virtual void DebugFunc()
    { }

#endif

    #endregion

    #endregion

}
