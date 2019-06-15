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

    public bool awake = true;

    private Transform player;
    private Transform head;
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
        SetComponents();
        InitializeBehaviorValues();
        lastKnowPlayerPos = transform.position;
        InvokeRepeating("ManageAwareness", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        ManageBehaviorTowardsPlayer();
    }

    // -- SETTERS ----------
    void SetComponents()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        head = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        brain = GetComponentInChildren<BeingHead>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Head")
            {
                head = child;
            }
        }
    }

    void InitializeBehaviorValues()
    {
        SetViewAngle();
        SetViewDistance();
        SetSuspiciousness();
        SetSpeed();
    }

    void SetViewAngle() {
        List<int> values = new List<int>();
        values.Add(Random.Range(60, 160));
        values.Add(Random.Range(60, 120));
        values.Add(Random.Range(60, 100));
        values.Add(Random.Range(60, 90));
        values.Add(Random.Range(40, 80));
        viewAngle = BehaviorHelper.QuickIntSwitch(age, ageBreakpoints, values);
    }

    void SetViewDistance()
    {
        List<int> values = new List<int>();
        values.Add(100);
        values.Add(120);
        values.Add(100);
        values.Add(80);
        values.Add(60);
        viewDistance = BehaviorHelper.QuickIntSwitch(age, ageBreakpoints, values);
    }

    void SetSuspiciousness() 
    {
        List<int> values = new List<int>();
        values.Add(30);
        values.Add(50);
        values.Add(30);
        values.Add(20);
        values.Add(10);
        suspiciousness = BehaviorHelper.QuickIntSwitch(age, ageBreakpoints, values);
    }

    void SetSpeed()
    {
        List<int> values = new List<int>();
        values.Add(8);
        values.Add(6);
        values.Add(5);
        values.Add(4);
        values.Add(3);
        agent.speed = BehaviorHelper.QuickIntSwitch(age, ageBreakpoints, values);
    }

    void ManageAwareness()
    {
        if (CanSeePlayer())
        {
            lastKnowPlayerPos = player.position;
            prevSuspicion = suspicion;
            suspicion += suspiciousness;
            float distanceFromPlayer = Vector3.Distance(transform.position, player.position);
            int distanceMultipler = (int)Mathf.Floor(2000f / distanceFromPlayer);
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
            if (suspicion > 95)
            {
                brain.OnSite();
            }
            if (suspicion > 100)
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
        float distanceFromPlayer = Vector3.Distance(head.position, player.position);
        Vector3 playerDirection = player.position - head.position;
        float angleOfView = Vector3.Angle(playerDirection, head.forward);
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
        if (head)
        {
            Gizmos.DrawWireSphere(head.position, viewDistance);
            Gizmos.DrawLine(head.position, transform.position + transform.forward * viewDistance);
        }
    }

}
