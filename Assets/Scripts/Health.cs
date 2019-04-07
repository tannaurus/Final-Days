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
        // Grab the rigidbody for later.
        rb = GetComponent<Rigidbody>();
        // Update the material to the default skin, just to be sure.
        UpdateMaterialTo(defaultSkin);
    }

    // Throw the rb in the specifed direction.
    // This is typically invoked via the weapon script.
    void GetThrown(MeleeAttack.Throw attackThrow)
    {  
        rb.AddForce(attackThrow.dir * attackThrow.force);
    }

    // Remove the specified "damage" from "health", flash the materials, and destroy the gameObject if it is out of health.
    // This is typically invoked via the weapon script.
    void TakeDamage(float damage)
    {
        health -= damage;
        StartCoroutine(FlashMaterial());
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    // A helper function that will update all the Game Object's children's material to the specified skin.
    // This will likely need to be refactored in the future as it updates all the children.
    void UpdateMaterialTo(Material mat)
    {
        Renderer[] childrenRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in childrenRenderers)
        {
            renderer.sharedMaterial = mat;
        }
    }

    // A Coroutine that will first apply the damaged skin, wait the allotted time, then apply the default skin.
    IEnumerator FlashMaterial()
    {
        UpdateMaterialTo(damagedSkin);
        yield return new WaitForSeconds(damageTime);
        UpdateMaterialTo(defaultSkin);
    }
}
