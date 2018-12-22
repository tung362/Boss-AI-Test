/*
 *      HUD
 *      
 *      Purpose
 *          Used in controlling all HUD elements.
 *          Persists throughout scenes.
 *          
 *      Dependencies
 *          SingletonBehaviour
 *          LoadScreen
 *          SceneLoader
 *          
 *      Accessors
 *          SceneLoader
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD : SingletonBehaviour<HUD>
{

    #region Variables

    #region Components

    // Used in turning mouse clicks into raycasts.
    // Disabled while loading.
    GraphicRaycaster caster;

    // Our load screen.
    public LoadScreen loadScreen;

    #endregion

    #region HUDObjects

    // All the menu gameobjects.

    public GameObject mainMenu;
    public GameObject hostMenu;
    public GameObject joinMenu;
    public GameObject lobbyMenu;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public GameObject hud;

    #endregion

    #region Properties

    // Can we interact with the menu currently?
    public bool interactable
    { get { return !SceneLoader.instance.loading; } }

    #endregion

    #endregion

    #region Methods

    #region Unity

    // Used for initialization.
    void Start()
    { caster = GetComponent<GraphicRaycaster>(); SceneManager.activeSceneChanged += LevelLoaded; }

    // Called every frame.
    void Update()
    { caster.enabled = interactable; }

    #endregion

    #region Commands

    // Turn off all menu objects.
    public void DisableMenus()
    {
        DisableMenu(mainMenu);
        DisableMenu(hostMenu);
        DisableMenu(joinMenu);
        DisableMenu(lobbyMenu);
        DisableMenu(pauseMenu);
        DisableMenu(optionsMenu);
        DisableMenu(creditsMenu);
        DisableMenu(hud);
    }

    // Enable specified menu.
    public void EnableMenu(GameObject menu)
    { menu.SetActive(true); }

    // Disable specified menu.
    public void DisableMenu(GameObject menu)
    { menu.SetActive(false); }

    #endregion

    #region Hooks

    // When the level is loaded, disable all menus and activate the appropriate ones based on scene.
    void LevelLoaded(Scene a, Scene b)
    {
        DisableMenus();
        if (b.name == "MainMenu") EnableMenu(mainMenu);
        else if (b.name == "Lobby") EnableMenu(lobbyMenu);
        else if (b.name == "Game") EnableMenu(hud);
    }

    #endregion

    #endregion

}
