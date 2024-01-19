using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedFall : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rig;
    GameObject rocket;
    [SerializeField] float fallSpeed;
    

    void Start()
    {

        rig = GetComponent<Rigidbody>();
        rocket = GameObject.Find("Rocket Ship");

        fallSpeed = 70f;

        rig.useGravity = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (rocket.transform.position.x >= this.transform.position.x && rocket.transform.position.y <= this.transform.position.y)
        {

            

            rig.AddForce(-Vector3.up * fallSpeed);
            
        } 


    }
}
