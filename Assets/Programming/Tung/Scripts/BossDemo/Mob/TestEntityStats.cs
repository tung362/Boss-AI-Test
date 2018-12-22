using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEntityStats : MonoBehaviour
{
    public float CurrentHealth = 100;
    public float MaxHealth = 100;
    public float Damage = 25;
    public bool IsDead = false;
    public bool IsPlayer = false;

    public float DamagedDuration = 0.5f;

    /*Data*/
    public List<MeshRenderer> Meshes = new List<MeshRenderer>();
    private List<Color> DefaultColors = new List<Color>();
    private bool Damaged = false;
    private float DamagedTimer = 0;

    void Start()
    {
        Meshes = FindAllMeshes(transform);
        if (GetComponent<MeshRenderer>() != null) Meshes.Add(GetComponent<MeshRenderer>());
        for (int i = 0; i < Meshes.Count; ++i) DefaultColors.Add(Meshes[i].material.color);
    }

    void Update()
    {
        if (Damaged)
        {
            DamagedTimer += Time.deltaTime;
            if (DamagedTimer >= DamagedDuration)
            {
                for (int i = 0; i < DefaultColors.Count; ++i) Meshes[i].material.color = DefaultColors[i];
                DamagedTimer = 0;
                Damaged = false;
            }
        }

        if (CurrentHealth <= 0) IsDead = true;
        if (CurrentHealth < 0) CurrentHealth = 0;
        if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
    }

    public void RecieveDamage(bool EntityType, float Damage, float Knockback)
    {
        if (EntityType) CurrentHealth -= Damage;
        else GetComponent<TestBossAI>().CurrentHealth -= Damage;

        for (int i = 0; i < Meshes.Count; ++i) Meshes[i].material.color = Color.red;
        Damaged = true;
    }

    public List<MeshRenderer> FindAllMeshes(Transform TheGameObject)
    {
        List<MeshRenderer> retval = new List<MeshRenderer>();
        foreach (Transform child in TheGameObject)
        {
            if (child.GetComponent<MeshRenderer>() != null) retval.Add(child.GetComponent<MeshRenderer>());
            retval.AddRange(FindAllMeshes(child));
        }
        return retval;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.layer == LayerMask.NameToLayer("MobHitbox") && IsPlayer) RecieveDamage(IsPlayer, 10, 20);
        if (other.transform.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox") && !IsPlayer) RecieveDamage(IsPlayer, 10, 10);
    }
}
