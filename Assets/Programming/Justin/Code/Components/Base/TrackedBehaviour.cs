/*
 *      TrackedBehaviour
 *      
 *      Purpose
 *          A variant of BaseBehaviour that will keep track of
 *          all components of this type.
 *          
 *      Dependencies
 *          BaseBehaviour
 *          
 *      Accessors
 *          ControlSource
 *          Actor
 */

using System.Collections.Generic;

public class TrackedBehaviour : BaseBehaviour
{

    protected static List<TrackedBehaviour> objects = new List<TrackedBehaviour>();

    protected override void Init()
    {
        base.Init();
        onAwake += AddToObjects;
        onDestroy += RemoveFromObjects;
    }

    void AddToObjects()
    { objects.Add(this); }

    void RemoveFromObjects()
    { objects.Remove(this); }
}
