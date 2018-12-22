using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BossAIType { Angry, Walk, AttackChain, ThrustAttack, BurstAttack };

public class TestBossAI : MonoBehaviour
{
    /*Setting*/
    [Header("General Settings")]
    public GameObject Player; //Ideally the closest player
    public GameObject FireSpot;
    public GameObject Flare;
    public GameObject Marker;
    public LineRenderer Tracer;
    public GameObject LeftAttackHitBox;
    public GameObject RightAttackHitBox;
    public GameObject OverHeadAttackHitBox;
    public GameObject ThrustAttackHitBox;
    public int CurrentStage = 1; //3 stages total
    public BossAIType CurrentBehaviour;
    public float MaxHealth = 100;
    public float CurrentHealth = 100;
    public int HealthStock = 2; //The amount of health bars
    public bool Invincible = false;
    //Laser skull settings
    public float LaserSkullActiveDuration = 15;
    public Vector2 LaserSkullActiveRandom = new Vector2(14, 17);
    public float LaserSkullCooldownDuration = 8;
    public Vector2 LaserSkullCooldownRandom = new Vector2(7, 11);
    //AI settings
    [Header("AI Settings")]
    public float TurnSpeed = 10;
    public float MoveSpeed = 150;
    public Vector3 CenterPosition = Vector3.zero; //The center of the map
    public float ChaseDistance = 1;
    public float CenterDistance = 0.05f;
    //Attack delays
    public float RightAttackDelay = 0.5f;
    public float LeftAttackDelay = 0.5f;
    public float OverheadAttackDelay = 0.5f;
    public float ThrustAttackDelay = 0.5f;
    public float BurstAttackDelay = 2;
    public float BurstAttackCooldownDelay = 20;
    //Lunge durations
    public float RightAttackLungeDuration = 0.2f;
    public float LeftAttackLungeDuration = 0.2f;
    public float OverHeadAttackLungeDuration = 0.2f;
    public float ThrustAttackLungeDuration = 0.2f;
    //Attack turn rates
    public float RightAttackTurnRate = 7;
    public float LeftAttackTurnRate = 7;
    public float OverHeadAttackTurnRate = 7;
    public float ThrustAttackTurnRate = 4;
    public float BurstAttackTurnRate = 10;


    /*Data*/
    UnityEngine.AI.NavMeshPath Path;
    private BossAIType PreviousBehaviour;
    private bool ChooseNewBehavior = true;
    private bool WentToCenter = false;
    //Laser skull timers
    private int PreviousStage = 0;
    private float LaserSkullActiveTimer = 0;
    private float LaserSkullCooldownTimer = 0;
    [HideInInspector]
    public bool SkullIsActive = false;
    //Attack timers
    private float RightAttackTimer = 0;
    private float LeftAttackTimer = 0;
    private float OverheadAttackTimer = 0;
    private float ThrustAttackTimer = 0;
    private float BurstAttackTimer = 0;
    private float BurstAttackCancelTimer = 0;
    //Lunge timers
    private float RightAttackLungeTimer = 0;
    private float LeftAttackLungeTimer = 0;
    private float OverHeadAttackLungeTimer = 0;
    private float ThrustAttackLungeTimer = 0;
    //Limits
    [HideInInspector]
    public bool RightAttackLungeLimit = false;
    [HideInInspector]
    public bool LeftAttackLungeLimit = false;
    [HideInInspector]
    public bool OverHeadAttackLungeLimit = false;
    [HideInInspector]
    public bool ThrustAttackLungeLimit = false;

    /*Required Components*/
    [Header("Required Components")]
    public Animator TheAnimator;
    private Rigidbody TheRigidbody;

    void Start()
    {
        TheRigidbody = GetComponent<Rigidbody>();

        Path = new UnityEngine.AI.NavMeshPath();

        PreviousBehaviour = CurrentBehaviour;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.N)) CurrentHealth -= 55;
        //Run order
        UpdateStage();
        UpdateHealth();
        PreviousBehaviour = CurrentBehaviour;
        UpdateBehavior();
        LaserSkull();
        PreviousStage = CurrentStage;
    }

    void UpdateHealth()
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        if(CurrentHealth <= 0)
        {
            if(!TheAnimator.GetBool("AngryToggle"))
            {
                if(HealthStock > 0)
                {
                    ResetAllAnimation();
                    Invincible = true;
                    CurrentBehaviour = BossAIType.Angry;
                    TheAnimator.SetBool("Angry", true);
                }
                TheAnimator.SetBool("AngryToggle", true);
            }
        }
    }

    void UpdateStage()
    {
        //Only runs when the AI is finished with it's current task
        if (!ChooseNewBehavior) return;
        switch (CurrentStage)
        {
            case 1:
                Stage1();
                break;
            case 2:
                Stage2();
                break;
            case 3:
                Stage3();
                break;
        }
    }

    void Stage1()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        //Walk
        if (distance > ChaseDistance && PreviousBehaviour != BossAIType.Walk) CurrentBehaviour = BossAIType.Walk;
        //Attack chain
        else
        {
            CurrentBehaviour = BossAIType.AttackChain;
            TheAnimator.SetBool("ChainAttack", true);
        }
    }

    void Stage2()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        int Chance = Random.Range((int)1, (int)5); //1-4

        if (Chance <= 3)
        {
            //Walk
            if (distance > ChaseDistance && PreviousBehaviour != BossAIType.Walk) CurrentBehaviour = BossAIType.Walk;
            //Attack chain
            else
            {
                CurrentBehaviour = BossAIType.AttackChain;
                TheAnimator.SetBool("ChainAttack", true);
            }
        }
        //Thrust attack
        else
        {
            CurrentBehaviour = BossAIType.ThrustAttack;
            TheAnimator.SetBool("ThrustAttack", true);
        }
    }

    void Stage3()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        int Chance = Random.Range((int)1, (int)6); //1-5

        if (Chance <= 3)
        {
            //Walk
            if (distance > ChaseDistance && PreviousBehaviour != BossAIType.Walk) CurrentBehaviour = BossAIType.Walk;
            //Attack chain
            else
            {
                CurrentBehaviour = BossAIType.AttackChain;
                TheAnimator.SetBool("ChainAttack", true);
            }
        }
        //Thrust attack
        else if (Chance == 4)
        {
            CurrentBehaviour = BossAIType.ThrustAttack;
            TheAnimator.SetBool("ThrustAttack", true);
        }
        else CurrentBehaviour = BossAIType.BurstAttack;
    }

    void UpdateBehavior()
    {
        //What is the current AI task
        switch (CurrentBehaviour)
        {
            case BossAIType.Angry:
                Angry();
                break;
            case BossAIType.Walk:
                Walk();
                break;
            case BossAIType.AttackChain:
                AttackChain();
                break;
            case BossAIType.ThrustAttack:
                ThrustAttack();
                break;
            case BossAIType.BurstAttack:
                BurstAttack();
                break;
        }
    }

    void Angry()
    {
        ChooseNewBehavior = false;

        if (!TheAnimator.GetBool("Angry"))
        {
            if (HealthStock > 0)
            {
                CurrentStage += 1;
                HealthStock -= 1;
                CurrentHealth = MaxHealth;
            }
            Invincible = false;
            TheAnimator.SetBool("AngryToggle", false);
            ChooseNewBehavior = true;
        }
    }

    void Walk()
    {
        ChooseNewBehavior = false;

        float distance = Vector3.Distance(transform.position, new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));

        //Walk
        if (distance > ChaseDistance)
        {
            //Calculate Path
            UnityEngine.AI.NavMesh.CalculatePath(transform.position, Player.transform.position, UnityEngine.AI.NavMesh.AllAreas, Path);

            //Apply motion
            if(Path.corners.Length > 1)
            {
                Vector3 direction = (new Vector3(Path.corners[1].x, transform.position.y, Path.corners[1].z) - transform.position).normalized;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), TurnSpeed * Time.fixedDeltaTime);
                //TheRigidbody.velocity = transform.forward * MoveSpeed * Time.fixedDeltaTime;
                TheRigidbody.velocity = direction * MoveSpeed * Time.fixedDeltaTime;
            }

            TheAnimator.SetBool("Walk", true);
        }
        //Idle
        else
        {
            //Exit
            TheAnimator.SetBool("Walk", false);

            //Apply motion
            TheRigidbody.velocity = Vector3.zero;

            ChooseNewBehavior = true;
            return;
        }
    }

    void AttackChain()
    {
        ChooseNewBehavior = false;

        //Exit
        if (!TheAnimator.GetBool("ChainAttack"))
        {
            ChooseNewBehavior = true;
            return;
        }

        Vector3 direction = (new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z) - transform.position).normalized;

        //Charge delays
        if (TheAnimator.GetCurrentAnimatorStateInfo(0).IsName("DungeonBossRightAttackCharge"))
        {
            //Apply rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), RightAttackTurnRate * Time.fixedDeltaTime);

            RightAttackTimer += Time.fixedDeltaTime;
            if(RightAttackTimer > RightAttackDelay && !TheAnimator.GetBool("RightAttackToggle"))
            {
                TheAnimator.SetBool("RightAttackToggle", true);
                RightAttackTimer = 0;
            }
        }
        if (TheAnimator.GetCurrentAnimatorStateInfo(0).IsName("DungeonBossLeftAttackCharge"))
        {
            //Apply rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), LeftAttackTurnRate * Time.fixedDeltaTime);

            LeftAttackTimer += Time.fixedDeltaTime;
            if (LeftAttackTimer > LeftAttackDelay && !TheAnimator.GetBool("LeftAttackToggle"))
            {
                TheAnimator.SetBool("LeftAttackToggle", true);
                LeftAttackTimer = 0;
            }
        }
        if (TheAnimator.GetCurrentAnimatorStateInfo(0).IsName("DungeonBossOverHeadAttackCharge"))
        {
            //Apply rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), OverHeadAttackTurnRate * Time.fixedDeltaTime);

            OverheadAttackTimer += Time.fixedDeltaTime;
            if (OverheadAttackTimer > OverheadAttackDelay && !TheAnimator.GetBool("OverheadAttackToggle"))
            {
                TheAnimator.SetBool("OverheadAttackToggle", true);
                OverheadAttackTimer = 0;
            }
        }

        //Limits
        if (RightAttackLungeLimit)
        {
            RightAttackHitBox.SetActive(true);

            RightAttackLungeTimer += Time.fixedDeltaTime;
            if (RightAttackLungeTimer >= RightAttackLungeDuration)
            {
                TheRigidbody.velocity = Vector3.zero;
                RightAttackLungeTimer = 0;
                RightAttackLungeLimit = false;
            }
        }
        else RightAttackHitBox.SetActive(false);

        if (LeftAttackLungeLimit)
        {
            LeftAttackHitBox.SetActive(true);

            LeftAttackLungeTimer += Time.fixedDeltaTime;
            if (LeftAttackLungeTimer >= LeftAttackLungeDuration)
            {
                TheRigidbody.velocity = Vector3.zero;
                LeftAttackLungeTimer = 0;
                LeftAttackLungeLimit = false;
            }
        }
        else LeftAttackHitBox.SetActive(false);

        if (OverHeadAttackLungeLimit)
        {
            OverHeadAttackHitBox.SetActive(true);

            OverHeadAttackLungeTimer += Time.fixedDeltaTime;
            if (OverHeadAttackLungeTimer >= OverHeadAttackLungeDuration)
            {
                TheRigidbody.velocity = Vector3.zero;
                OverHeadAttackLungeTimer = 0;
                OverHeadAttackLungeLimit = false;
            }
        }
        else OverHeadAttackHitBox.SetActive(false);
    }

    void ThrustAttack()
    {
        ChooseNewBehavior = false;

        //Exit
        if (!TheAnimator.GetBool("ThrustAttack"))
        {
            ChooseNewBehavior = true;
            return;
        }

        Vector3 direction = (new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z) - transform.position).normalized;

        //Charge delays
        if (TheAnimator.GetCurrentAnimatorStateInfo(0).IsName("DungeonBossThrustCharge"))
        {
            //Apply rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), ThrustAttackTurnRate * Time.fixedDeltaTime);

            ThrustAttackTimer += Time.fixedDeltaTime;
            if (ThrustAttackTimer > ThrustAttackDelay && !TheAnimator.GetBool("ThrustAttackToggle"))
            {
                TheAnimator.SetBool("ThrustAttackToggle", true);
                ThrustAttackTimer = 0;
            }
        }

        //Limits
        if (ThrustAttackLungeLimit)
        {
            OverHeadAttackHitBox.SetActive(true);
            ThrustAttackLungeTimer += Time.fixedDeltaTime;
            if (ThrustAttackLungeTimer >= ThrustAttackLungeDuration)
            {
                TheRigidbody.velocity = Vector3.zero;
                ThrustAttackLungeTimer = 0;
                ThrustAttackLungeLimit = false;
            }
        }
        else OverHeadAttackHitBox.SetActive(false);
    }

    void BurstAttack()
    {
        ChooseNewBehavior = false;

        float distance = Vector3.Distance(transform.position, new Vector3(CenterPosition.x, transform.position.y, CenterPosition.z));

        //Walk to center
        if (distance > CenterDistance && !WentToCenter)
        {
            //Calculate Path
            UnityEngine.AI.NavMesh.CalculatePath(transform.position, CenterPosition, UnityEngine.AI.NavMesh.AllAreas, Path);

            //Apply motion
            if (Path.corners.Length > 1)
            {
                Vector3 direction = (new Vector3(Path.corners[1].x, transform.position.y, Path.corners[1].z) - transform.position).normalized;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), TurnSpeed * Time.fixedDeltaTime);
                //TheRigidbody.velocity = transform.forward * MoveSpeed * Time.fixedDeltaTime;
                TheRigidbody.velocity = direction * MoveSpeed * Time.fixedDeltaTime;
            }

            TheAnimator.SetBool("Walk", true);
        }
        //Idle
        else
        {
            TheAnimator.SetBool("Walk", false);
            TheAnimator.SetBool("BurstAttack", true);
            WentToCenter = true;

            //Apply motion
            TheRigidbody.velocity = Vector3.zero;
            Vector3 direction = (new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z) - transform.position).normalized;

            //Defaults
            Tracer.SetPosition(0, FireSpot.transform.position);
            Tracer.SetPosition(1, FireSpot.transform.position);

            if(TheAnimator.GetBool("BurstAttackToggle")) FireSpot.SetActive(false);
            else FireSpot.SetActive(true);

            //Charge delays
            if (TheAnimator.GetCurrentAnimatorStateInfo(0).IsName("DungeonBossBurstCharge") || TheAnimator.GetCurrentAnimatorStateInfo(0).IsName("DungeonBossBurstRepeat"))
            {

                //Apply rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), BurstAttackTurnRate * Time.fixedDeltaTime);

                //Raycast and apply laser on closest hit
                RaycastHit[] hits = Physics.RaycastAll(FireSpot.transform.position, FireSpot.transform.forward);
                float closestDistance = int.MaxValue;
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.gameObject.layer == LayerMask.NameToLayer("Player") || hits[i].transform.gameObject.layer == LayerMask.NameToLayer("Enviornment") && !hits[i].collider.isTrigger)
                    {
                        float hitDistance = Vector3.Distance(transform.position, hits[i].point);
                        if (closestDistance > hitDistance)
                        {
                            closestDistance = hitDistance;
                            Tracer.SetPosition(1, hits[i].point);
                            Flare.SetActive(true);
                            Flare.transform.position = hits[i].point;
                            Marker.SetActive(true);
                            Marker.transform.position = new Vector3(hits[i].point.x, 0.1f, hits[i].point.z);
                        }
                    }
                }

                SkullIsActive = true;
                LaserSkullActiveTimer = 0;

                BurstAttackTimer += Time.fixedDeltaTime;
                Marker.GetComponent<TestLaserSkullMarker>().CurrentValue = BurstAttackTimer / BurstAttackDelay;
                if (BurstAttackTimer > BurstAttackDelay && !TheAnimator.GetBool("BurstAttackToggle"))
                {
                    TheAnimator.SetBool("BurstAttackToggle", true);
                    BurstAttackTimer = 0;
                }
            }

            //Exit
            if (TheAnimator.GetBool("BurstAttack"))
            {
                BurstAttackCancelTimer += Time.fixedDeltaTime;
                if(BurstAttackCancelTimer >= BurstAttackCooldownDelay)
                {
                    WentToCenter = false;
                    FireSpot.SetActive(false);
                    Marker.SetActive(false);
                    TheAnimator.SetBool("BurstAttack", false);
                    BurstAttackCancelTimer = 0;
                    ChooseNewBehavior = true;
                }
            }
        }
    }

    void LaserSkull()
    {
        if (CurrentStage < 2) return;

        if(SkullIsActive)
        {
            LaserSkullActiveTimer += Time.fixedDeltaTime;
            if(LaserSkullActiveTimer >= LaserSkullActiveDuration)
            {
                LaserSkullActiveDuration = Random.Range(LaserSkullActiveRandom.x, LaserSkullActiveRandom.y);
                LaserSkullActiveTimer = 0;
                SkullIsActive = false;
            }
        }
        else
        {
            LaserSkullCooldownTimer += Time.fixedDeltaTime;
            if (LaserSkullCooldownTimer >= LaserSkullCooldownDuration)
            {
                LaserSkullCooldownDuration = Random.Range(LaserSkullCooldownRandom.x, LaserSkullCooldownRandom.y);
                LaserSkullCooldownTimer = 0;
                SkullIsActive = true;
            }
        }
    }

    void ResetAllAnimation()
    {
        TheAnimator.SetBool("Walk", false);
        TheAnimator.SetBool("ChainAttack", false);
        TheAnimator.SetBool("ThrustAttack", false);
        TheAnimator.SetBool("ThrustAttackToggle", false);
        TheAnimator.SetBool("RightAttackToggle", false);
        TheAnimator.SetBool("LeftAttackToggle", false);
        TheAnimator.SetBool("OverheadAttackToggle", false);
        TheAnimator.SetBool("BurstAttack", false);
        TheAnimator.SetBool("BurstAttackToggle", false);
        TheAnimator.SetBool("Angry", false);


        Tracer.SetPosition(0, FireSpot.transform.position);
        Tracer.SetPosition(1, FireSpot.transform.position);
        Marker.SetActive(false);
        Flare.SetActive(false);
        FireSpot.SetActive(false);
    }
}
