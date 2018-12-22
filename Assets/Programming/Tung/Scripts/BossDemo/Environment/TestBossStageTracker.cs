using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossStageTracker : MonoBehaviour
{
    public List<TestBossPillar> Pillars;
    public List<TestlaserSkull> LaserSkulls;

    /*Data*/
    private TestBossAI Boss;
    private bool PreviousStatus = false;
    private int PreviousStage = 0;

    void Start()
    {

    }

    void Update()
    {
        FindBoss();
        UpdatePillar();
        UpdateSkull();
    }

    void FindBoss()
    {
        if (Boss == null)
        {
            Boss = FindObjectOfType<TestBossAI>();
            if(Boss != null)
            {
                if (Boss.SkullIsActive) PreviousStatus = false;
                else PreviousStatus = true;
            }
        }
    }

    void UpdatePillar()
    {
        if (Boss == null) return;
        if (Boss.CurrentStage != PreviousStage && Boss.CurrentStage > 1)
        {
            for (int i = 0; i < Pillars.Count; i++) Pillars[i].Toggle(true);
            PreviousStage = Boss.CurrentStage;
        }
    }

    void UpdateSkull()
    {
        if (Boss == null) return;
        if (Boss.SkullIsActive)
        {
            if(!PreviousStatus)
            {
                for (int i = 0; i < LaserSkulls.Count; i++) LaserSkulls[i].AttackToggle(true);
                PreviousStatus = true;
            }
        }
        else
        {
            if (PreviousStatus)
            {
                for (int i = 0; i < LaserSkulls.Count; i++) LaserSkulls[i].AttackToggle(false);
                PreviousStatus = false;
            }
        }
    }
}
