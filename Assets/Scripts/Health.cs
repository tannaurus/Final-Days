using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public Material defaultSkin;
    public Material damagedSkin;
    public float damageTime = 0.2f;

    public float health = 100f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateMaterialTo(defaultSkin);
    }

    void GetThrown(MeleeAttack.Throw attackThrow)
    {  
        rb.AddForce(attackThrow.dir * attackThrow.force);
    }


    void TakeDamage(float damage)
    {
        health -= damage;
        StartCoroutine(FlashMaterial());
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void UpdateMaterialTo(Material mat)
    {
        Renderer[] childrenRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in childrenRenderers)
        {
            renderer.sharedMaterial = mat;
        }
    }

    IEnumerator FlashMaterial()
    {
        UpdateMaterialTo(damagedSkin);
        yield return new WaitForSeconds(damageTime);
        UpdateMaterialTo(defaultSkin);
    }
}
