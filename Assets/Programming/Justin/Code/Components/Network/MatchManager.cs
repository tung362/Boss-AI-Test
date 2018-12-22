/*
 *      MatchManager
 *      
 *      Purpose
 *          Used to manage match information and connections as well as server commands.
 *          
 *      Dependencies
 *          SceneLoader
 *          
 *      Accessors
 *          Network UI Components
 *          SceneLoader
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MatchManager : NetworkLobbyManager
{

    #region Variables

    #region Components

    private static MatchManager _instance;
    public static MatchManager instance
    { get { return _instance; } }

    #endregion

    #region Prefabs

    public GameObject skeletonActor;

    #endregion

    #endregion

    #region Methods

    #region Setup

    // Used for initialization
    void Awake()
    {
        // Create singleton
        if (instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    #endregion

    #region Commands

    // Tell the scene manager to load a scene on the server
    public void LoadSceneOnServer(string scene)
    { ServerChangeScene(scene); }

    // Stop host/client and go back to main menu
    public void Disconnect()
    { StopHost(); }

    public void Host()
    { lobbyScene = "Lobby"; StartCoroutine(DelayHost()); }

    public void Join()
    { lobbyScene = "Lobby"; StartCoroutine(DelayJoin()); }

    #endregion

    #region Coroutines

    IEnumerator DelayHost()
    {
        SceneLoader.instance.LoadScene("Lobby");
        while (SceneManager.GetActiveScene().name != "Lobby") yield return new WaitForEndOfFrame();
        StartHost();
    }

    IEnumerator DelayJoin()
    {
        SceneLoader.instance.LoadScene("Lobby");
        while (SceneManager.GetActiveScene().name != "Lobby") yield return new WaitForEndOfFrame();
        StartClient();
    }

    #endregion

    // Called on the host when the connection starts
    public override void OnLobbyStartHost()
    {
        base.OnLobbyStartHost();

        // If the connection goes through, load the lobby scene and add the player
        if (IsClientConnected())
        {
            HUD.instance.DisableMenus();
            HUD.instance.EnableMenu(HUD.instance.lobbyMenu);
        }

        // TODO: Else, display connection error
    }

    // Called on the client when the connection starts
    public override void OnLobbyStartClient(NetworkClient lobbyClient)
    {
        base.OnLobbyStartClient(lobbyClient);

        // If the connection goes through, load the lobby scene and add the player
        if (IsClientConnected())
        {
            HUD.instance.DisableMenus();
            HUD.instance.EnableMenu(HUD.instance.lobbyMenu);
        }

        // TODO: Else, display connection error
    }

    public override void OnLobbyStopHost()
    {
        lobbyScene = "MainMenu";
        base.OnLobbyStopHost();
        HUD.instance.DisableMenus();
    }

    public override void OnLobbyStopClient()
    {
        lobbyScene = "MainMenu";
        base.OnLobbyStopClient();
        HUD.instance.DisableMenus();
    }

    // Called on the client when the scene changes
    public override void OnLobbyClientSceneChanged(NetworkConnection conn)
    {
        base.OnLobbyClientSceneChanged(conn);
    }

    #endregion

}
