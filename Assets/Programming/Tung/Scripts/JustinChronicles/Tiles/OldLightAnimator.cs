using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldLightAnimator : MonoBehaviour
{

    public float intensityMin;
    public float intensityMax;
    public float rangeMin;
    public float rangeMax;
    public Color colorA;
    public Color colorB;

    public float length;
    float timer = 0;
    bool reverse;

    float desiredIntensity
    { get { return Mathf.Lerp(intensityMin, intensityMax, timer / length); } }

    float desiredRange
    { get { return Mathf.Lerp(rangeMin, rangeMax, timer / length); } }

    Color desiredColor
    { get { return Color.Lerp(colorA, colorB, timer / length); } }

    new Light light;

    private void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        StepTimer();
        SetValues();
    }

    void SetValues()
    {
        light.intensity = desiredIntensity;
        light.range = desiredRange;
        light.color = desiredColor;
    }

    void StepTimer()
    {
        if (!reverse)
        {
            timer += Time.deltaTime;
            if (timer >= length)
            {
                timer = length;
                reverse = true;
            }
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                reverse = false;
            }
        }
    }
}