/*
 *      SingletonBehaviour
 *      
 *      Purpose
 *          To ensure only one of this object ever exists.
 *          The object persists through-out scenes unless destroyed.
 *          
 *      Dependencies
 *          None
 *          
 *      Accessors
 *          HUD
 */

using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
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

    // The instance of this object. Accessible from anywhere, if it exists.
    private static T _instance;
    public static T instance
    { get { return _instance; } }

    #endregion

    #region Methods

    // Used for pre enabled object set-up
    protected virtual void Awake()
    { EditorCheck(); SingletonCheck(); }

    // If we're not in the editor, ensure there's no debugging.
    void EditorCheck()
    {
#if UNITY_EDITOR
#else
        debug = 0;
#endif
    }

    void SingletonCheck()
    {
        // If there's not an instance yet...
        if (_instance == null)
        {
            // This is now the instance and it will persist through scenes.
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }

        // Otherwise, if there's an instance, destroy this object.
        else Destroy(gameObject);
    }

#endregion

}
