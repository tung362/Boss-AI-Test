/*
 *      Actor
 *      
 *      Purpose
 *          To set-up all actors appropriately and sync them across the network.
 *          Restricts behaviour based on components attached.
 *          
 *      Dependencies
 *          TrackedBehaviour
 *          
 *      Accessors
 *          None
 */

using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Actor : TrackedBehaviour
{

    #region Variables

    #region Static

    // List of all actors accessible to other classes.
    public static List<Actor> actors
    { get { return objects.OfType<Actor>().ToList(); } }

    public static List<Actor> localActors
    { get { return actors.Where(x => x.hasAuthority).ToList(); } }

    #endregion

    #region Components

    // The source controlling this object
    public ControlSource controller;

    // Replace the deprecated component with a reference to this objects rigidbody
    new protected Rigidbody rigidbody;

    // A component referencing the real animator that syncs animator variables online
    protected NetworkAnimator animator;

    #endregion

    #region Modifiers

    int localActorNumber = -1;

    public float speed = 1;

    // Max acceleration magnitude
    public float accelerationCap = 5f;

    // Max velocity magnitude
    public float velocityCap = 10f;

    #endregion

    #endregion

    #region Methods

    #region Setup

    protected override void Init()
    {
        base.Init();
        onAwake += ComponentCheck;
    }

    public void SetController(ControlSource ctrl)
    { controller = ctrl; AuthorityCheck(); }

    // Check which components we have and set them up if they are present.
    void ComponentCheck()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<NetworkAnimator>();
    }

    void AuthorityCheck()
    {
        if (controller != null && controller.hasAuthority)
        {
            if (rigidbody != null) InitMotor();
            if (animator != null) InitAnimator();
        }
    }

    // Physics set-up
    void InitMotor()
    {
        onFixedUpdate += ApplyVelocityCap;
    }

    void InitAnimator()
    { onFixedUpdate += AnimationUpdate; }

    #endregion

    #region Motor

    // If our velocity is past the max speed then clamp the magnitude.
    void ApplyVelocityCap()
    { rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, velocityCap); }

    public void Move(Vector2 input)
    {
        if (rigidbody)
        {
            // How much we desire to accelerate
            Vector3 acc = new Vector3(input.x, 0, input.y) * accelerationCap;

            // The difference between our accerlation and velocity
            Vector3 velChange = (acc - rigidbody.velocity);

            // Clamp the acceleration based on speed and move toward the acceleration cap
            velChange.x = Mathf.Clamp(velChange.x, -speed, speed);
            velChange.z = Mathf.Clamp(velChange.z, -speed, speed);
            velChange.y = 0;

            // Apply the velocity change
            rigidbody.AddForce(velChange, ForceMode.VelocityChange);
        }
    }

    #endregion

    protected virtual void AnimationUpdate()
    {

    }

    #endregion
}
