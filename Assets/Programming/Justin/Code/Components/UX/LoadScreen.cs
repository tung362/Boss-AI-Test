/*
 *      LoadScreen
 *      
 *      Purpose
 *          Used for controlling the loading screen automatically.
 *          
 *      Dependencies
 *          SceneLoader
 *          
 *      Accessors
 *          SceneLoader
 *          HUD
 */

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LoadScreen : MonoBehaviour
{

    #region Variables

    #region Components

    // The image of the load screen
    Image loadImage;

    #endregion

    #region Modifiers

    // Should it start faded opaque?
    public bool startFaded;

    // How long does it take to fade?
    public float length;
    float timer;

    #endregion

    #region Properties

    // Percentage value of loading progress
    public float progress
    { get { return timer / length; } }

    // Alpha of our loading screen image
    float alpha
    {
        get { return loadImage.color.a; }
        set { loadImage.color = new Color(loadImage.color.r, loadImage.color.g, loadImage.color.b, value); }
    }

    // Are we currently loading?
    bool loading
    { get { return SceneLoader.instance.loading; } }

    #endregion

    #endregion

    #region Methods

    #region Unity

    // Use this for initialization
    void Start ()
    { loadImage = GetComponent<Image>(); if (startFaded) { timer = length; SetColor(); } }
	
	// Update is called once per frame
	void Update ()
    { SetTimer(); SetColor(); }

    #endregion

    #region Commands

    // Used for updating the timer based on if you're loading or not
    void SetTimer()
    {
        if (loading) timer = Mathf.Clamp(timer + Time.deltaTime, 0, length);
        else timer = Mathf.Clamp(timer - Time.deltaTime, 0, length);
    }

    // Sets the color based on load progress
    void SetColor()
    { alpha = progress; }

    #endregion

    #endregion

}
