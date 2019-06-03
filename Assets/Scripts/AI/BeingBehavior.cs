using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeingBehavior : MonoBehaviour
{
    private Transform player;
    public bool awake = true;
    public float viewDistance = 10f;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (awake)
        {
            RotateBody();
        }
    }

    void RotateBody()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }

}
