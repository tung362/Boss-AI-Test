/*
 *      BaseBehaviour
 *      
 *      Purpose
 *          All network components should inherit from this.
 *          It will keep organization tidy and things easy to read.
 *          It was made in hopes of keeping code conflict from happening.
 *          Also it makes networking code easier to manage.
 *          
 *      Dependencies
 *          None
 *          
 *      Accessors
 *          TrackedBehaviour
 */

using System;
using UnityEngine;
using UnityEngine.Networking;

public class BaseBehaviour : NetworkBehaviour
{

    #region Variables

    #region Debug

    [SerializeField]
#if UNITY_EDITOR
    [EnumFlags]
#endif
    protected DebugType debug;

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

    #endregion

    #region Actions

    // Delegates used to send commands based on classes that inherit this script.
    // More info for each delegate in their respective methods.

    #region Basic

    protected Action onAwake;
    protected Action onStart;
    protected Action onDestroy;

    protected Action onUpdate;
    protected Action onFixedUpdate;
    protected Action onLateUpdate;

    #endregion

    #region Collision

    protected Action<Collision> onCollisionEnter;
    protected Action<Collision> onCollisionStay;
    protected Action<Collision> onCollisionExit;

    protected Action<Collider> onTriggerEnter;
    protected Action<Collider> onTriggerStay;
    protected Action<Collider> onTriggerExit;

    protected Action<Collision2D> onCollisionEnter2D;
    protected Action<Collision2D> onCollisionStay2D;
    protected Action<Collision2D> onCollisionExit2D;

    protected Action<Collider2D> onTriggerEnter2D;
    protected Action<Collider2D> onTriggerStay2D;
    protected Action<Collider2D> onTriggerExit2D;

    #endregion

    #endregion

    #endregion

    #region Methods

    #region Basic

    // Use this for pre object enabled set-up
    void Awake()
    { Init(); if(onAwake != null) onAwake(); }

    // Use this for initialization
    void Start()
    { if (onStart != null) onStart(); }
	
    void OnDestroy()
    { if (onDestroy != null) onDestroy(); }

	// Update is called once per frame
	void Update ()
    { if(onUpdate!= null) onUpdate(); }

    // FixedUpdate is called every physics update
    void FixedUpdate()
    { if(onFixedUpdate != null) onFixedUpdate(); }

    // LateUpdate is called before every render
    void LateUpdate()
    { if (onLateUpdate != null) onLateUpdate(); }

    #endregion

    #region Collision

    #region 3D

    // When a collision starts
    void OnCollisionEnter(Collision c)
    { if (onCollisionEnter != null) onCollisionEnter(c); }

    // During a collision
    void OnCollisionStay(Collision c)
    { if (onCollisionStay != null) onCollisionStay(c); }

    // At the end of a collision
    void OnCollisionExit(Collision c)
    { if (onCollisionExit != null) onCollisionExit(c); }

    // Upon entering a trigger
    void OnTriggerEnter(Collider c)
    { if (onTriggerEnter != null) onTriggerEnter(c); }

    // During collision with a trigger
    void OnTriggerStay(Collider c)
    { if (onTriggerStay != null) onTriggerStay(c); }

    // Upon exiting a trigger
    void OnTriggerExit(Collider c)
    { if (onTriggerExit != null) onTriggerExit(c); }

    #endregion

    #region 2D

    // When a collision starts
    void OnCollisionEnter2D(Collision2D c)
    { if (onCollisionEnter2D != null) onCollisionEnter2D(c); }

    // During a collision
    void OnCollisionStay2D(Collision2D c)
    { if (onCollisionStay2D != null) onCollisionStay2D(c); }

    // At the end of a collision
    void OnCollisionExit2D(Collision2D c)
    { if (onCollisionExit2D != null) onCollisionExit2D(c); }

    // Upon entering a trigger
    void OnTriggerEnter2D(Collider2D c)
    { if (onTriggerEnter2D != null) onTriggerEnter2D(c); }

    // During collision with a trigger
    void OnTriggerStay2D(Collider2D c)
    { if (onTriggerStay2D != null) onTriggerStay2D (c); }

    // Upon exiting a trigger
    void OnTriggerExit(Collider2D c)
    { if (onTriggerExit2D != null) onTriggerExit2D(c); }

    #endregion

    #endregion

    // Use this to set up child classes
    protected virtual void Init()
    {
        // If we're not in the editor, ensure there's no debugging.
#if UNITY_EDITOR
#else
        debug = 0;
#endif
    }

    #endregion

}
