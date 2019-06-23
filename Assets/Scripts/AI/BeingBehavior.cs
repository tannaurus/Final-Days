using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeingBehavior : MonoBehaviour
{
    // State
    private int suspicion = 0;
    private int prevSuspicion = 0;
    private int hunger = 100;
    private int thirst = 100;
    readonly int needsThreshold = 90;
    public BehaviorStates behaviorState = BehaviorStates.Wandering;

    // Behavior multipliers
    public int age = 35;

    public bool awake = true;
    public bool debug = false;

    private Transform player;
    private Transform head;
    private MemorySystem memorySystem;
    private Vector3 lastKnowPlayerPos;
    private NavMeshAgent agent;
    private BeingHead brain;
    private TextMesh debugTextMesh;
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
        InvokeRepeating("ManageNeeds", 0f, 1f);
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
        memorySystem = GetComponent<MemorySystem>();
        agent = GetComponent<NavMeshAgent>();
        brain = GetComponentInChildren<BeingHead>();
        head = GenericHelper.FindChildTransformWithTag("Head", transform);
        debugTextMesh = GenericHelper.FindChildTransformWithTag("DebugTextMesh", head).GetComponent<TextMesh>();
    }

    void InitializeBehaviorValues()
    {
        viewAngle = BeingHelper.DetermineViewAngle(age);
        viewDistance = BeingHelper.DetermineViewDistance(age);
        suspiciousness = BeingHelper.DetermineSuspiciousness(age);
        agent.speed = BeingHelper.DetermineSpeed(age);
    }

    // -- UPDATERS ----------
    void ManageAwareness()
    {
        if (CanSeePlayer())
        {
            lastKnowPlayerPos = player.position;
            prevSuspicion = suspicion;
            suspicion += suspiciousness;
            float distanceFromPlayer = Vector3.Distance(transform.position, player.position);
            int distanceMultipler = (int)Mathf.Floor(2000f / distanceFromPlayer);
            suspicion += distanceMultipler;
        } 
        else if (suspicion > 0)
        {
            suspicion -= suspiciousness;
        }
    }

    void ManageNeeds()
    {
        if (hunger != 0 && hunger > 1)
        {
            hunger--;
        }
        if (thirst != 0 & thirst > 2)
        {
            thirst = thirst - 2;
        }
        else
        {
            thirst = 0;
        }

        // We only want the being to look for needs if it has no suspicion
        // and isn't doing anything already.
        if (suspicion == 0)
        {
            if (thirst < needsThreshold & BehaviorHelper.CanPerformBehavior(BehaviorStates.LookingForWater, behaviorState))
            {
                GetWater();
            } else if (hunger < needsThreshold & BehaviorHelper.CanPerformBehavior(BehaviorStates.LookingForFood, behaviorState))
            {
                GetFood();
            }
        }

        if (debug)
        {
            debugTextMesh.text = "Hunger: " + hunger.ToString() + " - " + "Thirst: " + thirst.ToString();
        }
    }

    // -- MOVEMENT ----------
    void GetWater()
    {
        GameObject closestObj = memorySystem.FindNearestGameObjectInMemory("Drink", transform);
        if (closestObj == null)
        {
            return;
        }

        // No need to look again.
        if (behaviorState == BehaviorStates.LookingForWater)
        {
            if (BehaviorHelper.AtEndOfPath(agent))
            {
                int quench = closestObj.GetComponent<Drink>().value;
                memorySystem.RemoveGameObjectFromMemory(closestObj);
                agent.SetDestination(transform.position);
                thirst += quench;
                behaviorState = BehaviorStates.Wandering;
                Destroy(closestObj);
            }
            return;
        }

        behaviorState = BehaviorStates.LookingForWater;
        agent.SetDestination(closestObj.transform.position);
    }

    void GetFood()
    {
        GameObject closestObj = memorySystem.FindNearestGameObjectInMemory("Food", transform);
        if (closestObj == null)
        {
            return;
        }
        // No need to look again.
        if (behaviorState == BehaviorStates.LookingForFood)
        {
            if (BehaviorHelper.AtEndOfPath(agent))
            {
                int food = closestObj.GetComponent<Food>().value;
                memorySystem.RemoveGameObjectFromMemory(closestObj);
                agent.SetDestination(transform.position);
                hunger += food;
                behaviorState = BehaviorStates.Wandering;
                Destroy(closestObj);
            }
            return;
        }

        behaviorState = BehaviorStates.LookingForFood;
        agent.SetDestination(closestObj.transform.position);
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

    // -- DEBUG ----------
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if (debug & head != null)
        {
            Gizmos.DrawWireSphere(head.position, viewDistance);
            Gizmos.DrawLine(head.position, transform.position + transform.forward * viewDistance);
        }
    }

}
