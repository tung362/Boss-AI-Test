/*
 *      ControlSource
 *      
 *      Purpose
 *          Used for translating input to commands.
 *          
 *      Dependencies
 *          TrackedBehaviour
 *          
 *      Accessors
 *          Player
 *          AI
 */

using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ControlSource : TrackedBehaviour
{

    #region Variables

    #region Components

    // List of all control sources
    public static List<ControlSource> controlSources
    { get { return objects.OfType<ControlSource>().ToList(); } }

    // The current actor we're sending commands to
    Actor _character;
    public Actor character
    { get { return _character; } }

    #endregion

    #region Inputs

    // Our inputs used for sending commands

    protected Vector2 movement;

    protected bool interact;

    protected bool attack;
    protected bool item;
    protected bool special;

    #endregion

    #endregion

    #region Methods

    #region Setup

    // Perform initial set-up.
    protected override void Init()
    {
        base.Init();
        onStart += AuthorityCheck;
    }

    // Check if the object is owned by the client and set-up accordingly.
    void AuthorityCheck()
    {
        if (hasAuthority) onUpdate += ControlUpdate;
    }

    #endregion

    #region Controls

    // Called every update. Used for getting input and applying it to commands.
    void ControlUpdate()
    {
        ResetInput();
        GetInput();
        CallCommands();
    }

    // Zero out inputs
    void ResetInput()
    {
        movement = Vector3.zero;
        interact = false;
        attack = false;
        item = false;
        special = false;
    }

    // Get input from Player or AI
    protected virtual void GetInput()
    { }

    // Apply input to commands
    void CallCommands()
    {
        if(_character)
        {
            if(movement != Vector2.zero)
                _character.Move(movement);
        }
    }

    #endregion

    #region Commands

    // Used to set our current character we're controlling
    [ClientRpc]
    public void RpcSetCharacter(NetworkInstanceId obj)
    { _character = ClientScene.FindLocalObject(obj).gameObject.GetComponent<Actor>(); character.SetController(this); }

    #endregion

    #endregion

}
