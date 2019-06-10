using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeingBehavior : MonoBehaviour
{
    public bool awake = true;
    public float viewDistance = 100f;
    public float viewAngle = 60f;
    public float suspiciousness = 0.5f;

    private Transform player;
    private Vector3 lastKnowPlayerPos;
    private NavMeshAgent agent;
    private BeingHead brain;
    private float suspicion = 0f;
    private float prevSuspicion = 0f;
    
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        brain = GetComponentInChildren<BeingHead>();
        lastKnowPlayerPos = transform.position;
        InvokeRepeating("ManageSuspicion", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        ManageBehaviorTowardsPlayer();
    }

    void ManageSuspicion()
    {
        if (CanSeePlayer())
        {
            lastKnowPlayerPos = player.position;
            prevSuspicion = suspicion;
            suspicion += suspiciousness;
        } else if (suspicion > 0f)
        {
            suspicion -= suspiciousness;
        }
    }

    void ManageBehaviorTowardsPlayer()
    {
        Debug.Log(suspicion);
        if (CanSeePlayer())
        {
            if (suspicion > 3f)
            {
                brain.OnSite();
            }
            if (suspicion > 5f)
            {
                RotateBody();
                MoveTowardsPlayer();
            }
        }
        else
        {
            MoveTowardsLastKnownPlayerPos();
        }
    }

    bool CanSeePlayer()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, player.position);
        Vector3 playerDirection = player.position - transform.position;
        float angleOfView = Vector3.Angle(playerDirection, transform.forward);
        if (distanceFromPlayer <= viewDistance & angleOfView <= viewAngle & brain.PlayerInLineOfSight()) {
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

    void MoveTowardsPlayer()
    {
        agent.SetDestination(player.position);
    }

    void MoveTowardsLastKnownPlayerPos()
    {
        agent.SetDestination(lastKnowPlayerPos);
    }

    // DEBUG
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * viewDistance);
    }

}
