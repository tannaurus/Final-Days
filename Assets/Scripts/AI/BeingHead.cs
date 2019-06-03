using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeingHead : MonoBehaviour
{
    private Transform player;
    public bool awake = true;
    public float viewDistance = 100f;
    // public float speed = 1f;

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
            MonitorPlayer();
        }
    }

    void MonitorPlayer()
    {
        transform.LookAt(player.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * viewDistance);
    }

    //void IdleMovement()
    //{
    //    List<Quaternion> groove = IdleHeadMovements.GetGroove();
    //    RunMovement(groove);
    //}

    //void RunMovement(List<Quaternion> movement, int index = 0)
    //{
    //    float calculatedSpeed = speed * Time.deltaTime;
    //    Quaternion.RotateTowards(transform.rotation, movement[index], calculatedSpeed);
    //    if (index != movement.Count -1)
    //    {
    //        RunMovement(movement, index + 1);
    //    }
    //}

}

//class IdleHeadMovements
//{
//    public static List<Quaternion> GetGroove()
//    {
//        List<Quaternion> groove = new List<Quaternion>();
//        groove.Add(Quaternion.Euler(0, 0, 0));
//        groove.Add(Quaternion.Euler(50, 0, 0));
//        groove.Add(Quaternion.Euler(0, 0, 0));
//        groove.Add(Quaternion.Euler(-50, 0, 0));
//        return groove;
//    }
//}
