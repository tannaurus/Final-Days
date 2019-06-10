using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeingBehavior : MonoBehaviour
{
    private Transform player;
    private Vector3 lastKnowPlayerPos;
    private NavMeshAgent agent;
    private BeingHead brain;
    public bool awake = true;
    public float viewDistance = 100f;
    public float viewAngle = 60f;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        brain = GetComponentInChildren<BeingHead>();
        lastKnowPlayerPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (awake & CanSeePlayer())
        {
            RotateBody();
            brain.OnSite();
        }
        UpdateMovement();
    }

    bool CanSeePlayer()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, player.position);
        Vector3 playerDirection = player.position - transform.position;
        float angleOfView = Vector3.Angle(playerDirection, transform.forward);
        if (distanceFromPlayer <= viewDistance & angleOfView <= viewAngle & brain.HasEyesOnPlayer()) {
            lastKnowPlayerPos = player.position;
            return true;
        }

        return false;
    }

    void RotateBody()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void UpdateMovement()
    {
        if (CanSeePlayer())
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.SetDestination(lastKnowPlayerPos);
        }
    }

    // DEBUG
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * viewDistance);
    }

}
