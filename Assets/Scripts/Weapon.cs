using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float damage;
    public float range;
    public float attackAngle;
    public float speed;
    public Vector3 inHandPosition;
    public Vector3 inHandRotation;

    private bool isEquiped = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        if (isEquiped) 
        {
            UpdateWeaponPosition();
        }
    }

    void UpdateWeaponPosition()
    {
        transform.localPosition = inHandPosition;
        transform.localEulerAngles = inHandRotation;
    }

    void Equip(GameObject parent)
    {
        gameObject.transform.parent = parent.transform;
        UpdateWeaponPosition();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.None;
        isEquiped = true;
    }

    void Drop()
    {
        gameObject.transform.parent = null;
        rb.useGravity = true;
        isEquiped = false;
    }



}
