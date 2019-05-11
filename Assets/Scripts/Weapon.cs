using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {

    public float damage;
    public float range;
    public float attackAngle;
    public float speed;

    // Used as a structured way to inform the effected rigidbody that it's been hit
    // by an attackfrom a certain direction with the force specified
    public class Throw
    {
        public Vector3 dir;
        public float force;
        public Throw(Vector3 d, float f)
        {
            dir = d;
            force = f;
        }
    }



}
