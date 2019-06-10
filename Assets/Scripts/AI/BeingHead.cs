using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeingHead : MonoBehaviour
{
    private Transform player;
    // public float speed = 1f;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public bool PlayerInLineOfSight()
    {
        Vector3 playerDirection = player.position - transform.position;
        if (Physics.Raycast(transform.position, playerDirection, out RaycastHit hit))
        {
            Debug.Log(hit.transform.tag);
            return hit.transform.tag == "Player";
        }
        return false;
    }

    public void OnSite()
    {
        transform.LookAt(player.position);
    }

}

// BRING BACK AFTER MVP.
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
