using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using static BlueyController;

public class BalloonController : MonoBehaviour
{
    public BlueyController blueyController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Obsticle")
        {
            Debug.Log(collision.gameObject.name + " Collided!");
            blueyController.AddReward(-3);
        }
        if(collision.collider.tag == "Floor")
        {
            Debug.Log("Floor collided");
            blueyController.AddReward(-10);
            blueyController.EndEpisode(); 
        }
    }
}
