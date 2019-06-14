using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeingBehavior : MonoBehaviour
{
    // State
    private int suspicion = 0;
    private int prevSuspicion = 0;

    // Behavior multipliers
    public int age = 35;
    public int awareness = 100;
    public int sight = 100;
    public int insanity = 0;

    public bool awake = true;

    private Transform player;
    private Vector3 lastKnowPlayerPos;
    private NavMeshAgent agent;
    private BeingHead brain;
    readonly List<int> ageBreakpoints = BehaviorHelper.GetAgeBreakpoints();

    // Default values that will be calculated by other factors
    private int viewAngle = 60;
    private float viewDistance = 100f;
    private int suspiciousness = 1;

    // Use this for initialization
    void Start()
    {
        InitializeBehaviorValues();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        brain = GetComponentInChildren<BeingHead>();
        lastKnowPlayerPos = transform.position;
        InvokeRepeating("ManageAwareness", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        ManageBehaviorTowardsPlayer();
    }

    void InitializeBehaviorValues()
    {
        SetViewAngle();
        SetViewDistance();
    }

    void SetViewAngle() {
        List<int> values = new List<int>();
        values.Add(160);
        values.Add(120);
        values.Add(100);
        values.Add(90);
        values.Add(60);
        viewAngle = BehaviorHelper.QuickIntSwitch(age, ageBreakpoints, values);
    }

    void SetViewDistance()
    {
        List<int> values = new List<int>();
        values.Add(60);
        values.Add(120);
        values.Add(100);
        values.Add(80);
        values.Add(60);
        viewDistance = BehaviorHelper.QuickIntSwitch(age, ageBreakpoints, values);
        Debug.Log(viewDistance);
    }

    void ManageAwareness()
    {
        if (CanSeePlayer())
        {
            lastKnowPlayerPos = player.position;
            prevSuspicion = suspicion;
            suspicion += suspiciousness;
            float distanceFromPlayer = Vector3.Distance(transform.position, player.position);
            int distanceMultipler = (int)Mathf.Floor(distanceFromPlayer / 100f);
            Debug.Log(distanceMultipler);
            suspicion += distanceMultipler;
        } 
        else if (suspicion > 0)
        {
            suspicion -= suspiciousness;
        }
    }

    void ManageBehaviorTowardsPlayer()
    {
        if (CanSeePlayer())
        {
            if (suspicion > 3)
            {
                brain.OnSite();
            }
            if (suspicion > 4)
            {
                RotateBody();
                MoveTowardsPlayer();
            }
        }
        else if (prevSuspicion > 0 & suspicion == 0)
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
