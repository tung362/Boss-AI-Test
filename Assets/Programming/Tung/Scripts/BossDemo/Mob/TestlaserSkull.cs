using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestlaserSkull : MonoBehaviour
{
    /*Settings*/
    public GameObject Projectile;
    public TestLaserSkullSpinner Spinner;
    public GameObject Player; //Ideally the closest player
    public GameObject FireSpot;
    public GameObject FlareParticle;
    public GameObject Marker;
    public LineRenderer Tracer;
    //public LineRenderer FireTracer;
    public float FireDelay = 4;
    public Vector2 RandomizedFireDelay = Vector3.zero;
    public float TurnSpeed = 10;


    /*Data*/
    public bool Attack = false;
    private bool Tracing = true;
    private float FireTimer = 0;
    Vector3 HitPosition = Vector3.zero;

    /*Required Components*/
    private Animator TheAnimator;
    private RotateObject TheRotateObject;

    void Start()
    {
        TheAnimator = GetComponent<Animator>();
        TheRotateObject = GetComponent<RotateObject>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) AttackToggle();
        Defaults();
        UpdateAttack();
    }

    void Defaults()
    {
        Tracer.SetPosition(0, FireSpot.transform.position);
        Tracer.SetPosition(1, FireSpot.transform.position);

        //FireTracer.SetPosition(0, FireSpot.transform.position);
        //FireTracer.SetPosition(1, FireSpot.transform.position);

        FireSpot.SetActive(false);
        FlareParticle.SetActive(false);

        if (!Attack)
        {
            TheAnimator.SetBool("Trace", false);
            TheAnimator.SetBool("Blast", false);
            TheRotateObject.Toggle(true);
            Marker.SetActive(false);
            Spinner.Toggle(true);
        }
    }

    void UpdateAttack()
    {
        if (!Attack) return;

        FireSpot.SetActive(true);
        Marker.SetActive(true);
        Spinner.Toggle(false);
        TheRotateObject.Toggle(false);
        //Marker.transform.position = new Vector3(Player.transform.position.x, 0.1f, Player.transform.position.z);
        TheAnimator.SetBool("Trace", true);

        //Trace or fire
        if (Tracing)
        {
            //Tracking
            Vector3 Direction = (Player.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), TurnSpeed * Time.deltaTime);

            //Raycast and apply laser on closest hit
            RaycastHit[] hits = Physics.RaycastAll(FireSpot.transform.position, FireSpot.transform.forward);
            float closestDistance = int.MaxValue;
            for (int i = 0; i < hits.Length; i++)
            {
                if(hits[i].transform.gameObject.layer == LayerMask.NameToLayer("Player") || hits[i].transform.gameObject.layer == LayerMask.NameToLayer("Enviornment") && !hits[i].collider.isTrigger)
                {
                    float distance = Vector3.Distance(transform.position, hits[i].point);
                    if(closestDistance > distance)
                    {
                        closestDistance = distance;
                        Tracer.SetPosition(1, hits[i].point);
                        FlareParticle.SetActive(true);
                        FlareParticle.gameObject.transform.position = hits[i].point;
                        Marker.transform.position = new Vector3(hits[i].point.x, 0.1f, hits[i].point.z);
                        HitPosition = hits[i].point;
                    }
                }
            }

            //Fire Timer
            FireTimer += Time.deltaTime;
            Marker.GetComponent<TestLaserSkullMarker>().CurrentValue = FireTimer / FireDelay;
            if (FireTimer >= FireDelay)
            {
                TheAnimator.SetBool("Blast", true);
                FireDelay = Random.Range(RandomizedFireDelay.x, RandomizedFireDelay.y);
                Tracing = false;
                FireTimer = 0;
            }
        }
        else
        {
            //GameObject spawnedBullet = Instantiate(Projectile, FireSpot.transform.position, FireSpot.transform.rotation) as GameObject;
            //spawnedBullet.GetComponent<Rigidbody>().velocity = spawnedBullet.transform.forward * 2000 * Time.deltaTime;
            //FireTracer.SetPosition(1, HitPosition);
            if (!TheAnimator.GetBool("Blast")) Tracing = true;
        }
    }

    public void AttackToggle()
    {
        Attack = !Attack;
        FireTimer = 0;
        Tracing = true;
    }

    public void AttackToggle(bool TrueFalse)
    {
        Attack = TrueFalse;
        FireTimer = 0;
        Tracing = true;
    }
}
