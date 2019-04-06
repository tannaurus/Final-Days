using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectedAI : MonoBehaviour {

    private Transform player;
    public float movementSpeed = 10f;
    public float viewDistance = 10f;
    public float minDistance = 1f;

    public float health = 100f;

    // Use this for initialization
    void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        monitorPlayer();
	}

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.contacts[0].thisCollider.tag);
    }

    void monitorPlayer()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceFromPlayer <= viewDistance && distanceFromPlayer >= minDistance)
        {
            transform.LookAt(player);
            transform.position += transform.forward * movementSpeed;
        }
    }
}
