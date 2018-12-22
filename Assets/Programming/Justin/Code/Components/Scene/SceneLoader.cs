/*
 *      SceneLoader
 *      
 *      Purpose
 *          To allow proper scene loading asynchronously, Online and with load screens.
 *          
 *      Dependencies
 *          SingletonBehaviour
 *          HUD
 *          
 *      Accessors
 *          MatchManager
 *          HUD
 *          LoadScreen
 */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonBehaviour<SceneLoader>
{

    #region Variables

    #region Stats

    // Loading percentage if we're loading
    public float loadProgress;

    // Are we loading?
    bool _loading;
    public bool loading
    { get { return _loading; } }

    #endregion

    #endregion

    #region Methods

    #region Unity

    // Used for initialization.
    void Start()
    { SceneManager.sceneLoaded += LevelLoaded; }

    #endregion

    #region Commands

    // Load a scene on the client.
    public void LoadScene(string scene)
    { if (!loading) { StartCoroutine(AsyncLoadScene(scene)); } }

    // Load a scene on the server.
    public void ServerLoadScene(string scene)
    { if (!loading) { StartCoroutine(ServerLoadSceneOperation(scene)); } }

    #endregion

    #region Coroutines

    // The routine for loading a scene on the client.
    IEnumerator AsyncLoadScene(string scene)
    {
        // We're currently loading now.
        _loading = true;

        // Start the operation for loading the scene in the background.
        AsyncOperation aOp = SceneManager.LoadSceneAsync(scene);

        // Disallow activation of the scene.
        aOp.allowSceneActivation = false;

        // While we're loading and waiting for the load screen to fade in, update the progress and wait.
        while (aOp.progress < 0.9f || HUD.instance.loadScreen.progress < 1)
        { loadProgress = aOp.progress / 0.9f; yield return new WaitForEndOfFrame(); }

        // When we're done, allow scene activation.
        aOp.allowSceneActivation = true;

        // We're no longer loading.
        _loading = false;
    }

    // The routine for loading a scene on the server.
    IEnumerator ServerLoadSceneOperation(string scene)
    {
        // We're currently loading now.
        _loading = true;

        // While we're waiting for the load screen to fade in, update the progress and wait.
        while (HUD.instance.loadScreen.progress < 1)
            yield return new WaitForEndOfFrame();

        // Tell the server to load the scene.
        MatchManager.instance.LoadSceneOnServer(scene);

        // We're done loading.
        _loading = false;
    }

    #endregion

    #region Hooks

    // Called when a level is fully loaded.
    void LevelLoaded(Scene scene, LoadSceneMode mode)
    { loadProgress = 0; }

    #endregion

    #endregion

}
