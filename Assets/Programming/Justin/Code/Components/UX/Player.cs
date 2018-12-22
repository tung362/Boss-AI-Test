/*
 *      Player
 *      
 *      Purpose
 *          A variant of ControlSource used for player input.
 *          Also keeps track of player information.
 *          
 *      Dependencies
 *          ControlSource
 *          
 *      Accessors
 *          None
 */

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Player : ControlSource {

    #region Variables

    #region Static

    // List of all players available to other classes.
    public static List<Player> players
    { get { return objects.OfType<Player>().ToList(); } }

    public static List<Player> localPlayers
    { get { return players.Where(x => x.hasAuthority).ToList(); } }

    #endregion

    #region Net

    // The player who owns this object
    [SyncVar]
    public string owner;

    NetworkLobbyPlayer _lobbyPlayer;
    public NetworkLobbyPlayer lobbyPlayer
    { get { return _lobbyPlayer; } }
    
    #endregion

    #region Delegates

    // Delegates used to send commands based on classes that inherit this script.
    // More info for each delegate in their respective methods.

    protected delegate void NetworkDisconnectionDelegate(NetworkDisconnection nd);
    protected delegate void NetworkConnectionErrorDelegate(NetworkConnectionError nce);
    protected delegate void NetworkPlayerDelegate(NetworkPlayer np);

    protected Action onConnectedToServer;

    protected Action<NetworkDisconnection> onDisconnectedFromServer;

    protected Action<NetworkConnectionError> onFailedToConnect;
    protected Action<NetworkConnectionError> onFailedToConnectToMasterServer;

    protected Action<NetworkPlayer> onPlayerConnected;
    protected Action<NetworkPlayer> onPlayerDisconnected;

    #endregion

    #endregion

    #region Methods

    #region Setup

    // Used for initial set-up
    protected override void Init()
    { base.Init(); onStart += GetComponents; onStart += AuthorityCheck; }

    void GetComponents()
    { _lobbyPlayer = GetComponent<NetworkLobbyPlayer>(); }

    // Used to check if this object is owned locally. Set-up accordingly.
    void AuthorityCheck()
    {
        if(hasAuthority)
        {
            CmdSpawnSkeleton();
            onDisconnectedFromServer += DisconnectionHandler;
            SceneManager.sceneLoaded += SpawnSkeleton;
        }
    }

    // Used to spawn skeleton actor as our character when scene changes
    [Command]
    void CmdSpawnSkeleton()
    {
        GameObject skeleton = Instantiate(MatchManager.instance.skeletonActor,
                                          MatchManager.instance.startPositions[UnityEngine.Random.Range(0, MatchManager.instance.startPositions.Count - 1)].position,
                                          MatchManager.instance.startPositions[UnityEngine.Random.Range(0, MatchManager.instance.startPositions.Count - 1)].rotation);

        NetworkServer.SpawnWithClientAuthority(skeleton, connectionToClient);

        RpcSetCharacter(skeleton.GetComponent<NetworkIdentity>().netId);
    }

    void SpawnSkeleton(Scene scene, LoadSceneMode mode)
    {
        CmdSpawnSkeleton();
    }

    // Used for getting input to send commands from the control source
    protected override void GetInput()
    {
        base.GetInput();

        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        attack = Input.GetButtonDown("Fire1");
        item = Input.GetButtonDown("Fire2");
        special = Input.GetButtonDown("Fire3");
    }

    #endregion

    #region Net

    // Called when disconnected from server.
    void DisconnectionHandler(NetworkDisconnection nd)
    { Destroy(gameObject); }

    #endregion

    #region DelegateSetup

    #region Net

    // Called on the client when connected to a server
    void OnConnectedToServer()
    { if(onConnectedToServer != null) onConnectedToServer(); }

    // Called on the client when disconnected from a server
    void OnDisconnectedFromServer(NetworkDisconnection nd)
    { if(onDisconnectedFromServer != null) onDisconnectedFromServer(nd); }

    // Called when a connection attempt fails
    void OnFailedToConnect(NetworkConnectionError nce)
    { if(onFailedToConnect != null) onFailedToConnect(nce); }

    // Called when attempting to connect to the master server fails
    void OnFailedToConnectToMasterServer(NetworkConnectionError nce)
    { if(onFailedToConnectToMasterServer != null) onFailedToConnectToMasterServer(nce); }

    // Called on the server when a player connects
    void OnPlayerConnected(NetworkPlayer np)
    { owner = np.ipAddress; if(onPlayerConnected != null) onPlayerConnected(np); }

    // Called on the server when a player disconnects
    void OnPlayerDisconnected(NetworkPlayer np)
    { if(onPlayerDisconnected != null) onPlayerDisconnected(np); }

    #endregion

    #endregion

    #endregion

}
