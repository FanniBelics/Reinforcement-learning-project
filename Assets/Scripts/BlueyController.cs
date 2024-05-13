using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

public class BlueyController : Agent
{
    public float speed;
    private Vector3 startingPosition = new Vector3(0.0f, 10f, 0.0f);
    public Rigidbody rb;
    public Transform Balloon;
    private static BlueyController instance;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start...");
    }

    public override void OnEpisodeBegin()
    {

        transform.localPosition = new Vector3(0, 0.5f, 0);

        Debug.Log("Episode begins");
        //float xPosition = UnityEngine.Random.RandomRange(-1, 3);
        //float zPosition = UnityEngine.Random.RandomRange(-1, 3);
        transform.rotation = Quaternion.identity;

        //Vector3 position = new Vector3 (xPosition, 10, zPosition);

        Balloon.localPosition = transform.localPosition + startingPosition;

        Rigidbody balloonRigidbody = Balloon.GetComponent<Rigidbody>();

        if (balloonRigidbody != null)
        {
            balloonRigidbody.velocity = Vector3.zero;
            balloonRigidbody.angularVelocity = Vector3.zero;
        }

    }

    // Update is called once per frame
     /*void Update()
     {
         if (Input.GetKey("w"))
        {
             transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime * actionDirection)
         }
         if (Input.GetKey("d"))
             transform.Rotate(0.0f, +0.5f, 0.0f);

         if (Input.GetKey("a"))
             transform.Rotate(0.0f, -0.5f, 0.0f);

         if (Input.GetKey("s"))
         {
             transform.Translate(Vector3.back * speed * Time.fixedDeltaTime);
         }
     }*/

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Balloon.localPosition);
        sensor.AddObservation(Balloon.GetComponent<Rigidbody>().velocity.normalized);
        sensor.AddObservation(transform.rotation.normalized);
        sensor.AddObservation(transform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionTaken = actions.ContinuousActions;

        float actionDirection = actionTaken[0];
        float actionSteering = actionTaken[1];

        float moveInput = actions.ContinuousActions[0];
        float rotateInput = actions.ContinuousActions[1];

        rb.velocity = transform.forward * speed * moveInput;
        transform.Rotate(Vector3.up * 100f * rotateInput * Time.deltaTime);

        if(Balloon.transform.localPosition.y  > 1.5)
        {
            AddReward(0.001f);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> actions = actionsOut.ContinuousActions;

        actions[0] = 0f;
        actions[1] = 0f;

        actions[0] = Input.GetAxis("Vertical");
        actions[1] = Input.GetAxis("Horizontal");

        Debug.Log("Forward/Backward Action: " + actions[0]);
        Debug.Log("Turn Action: " + actions[1]);
    }
  

   /*public void EndThisEpisode()
    {
        AddReward(-10);
        EndEpisode();
    }

    public void CollidedWithObsticle()
    {
        AddReward(-3);
    }*/


    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with a furniture object
        if (collision.gameObject.CompareTag("Obsticle") ||collision.gameObject.CompareTag("Walls"))
        {
            // Stop the agent's movement
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            AddReward(-0.1f);
        }

        if (collision.gameObject.CompareTag("Balloon")){
            AddReward(10.0f);
        }
    }

    public void Update()
    {
        //float distance = Vector3.Distance(transform.localPosition, Balloon.localPosition);
        //AddReward(0.01f * distance);
    }
}
