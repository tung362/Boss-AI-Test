using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBossStock : MonoBehaviour
{
    public GameObject Boss;
    public int StockNumber = 0;

    private Image TheImage;

    void Start()
    {
        TheImage = GetComponent<Image>();
    }

    void Update()
    {
        if (Boss.GetComponent<TestBossAI>().HealthStock < StockNumber) TheImage.enabled = false;
        else TheImage.enabled = true;
    }
}
