using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

public class BlueyController : Agent
{
    public float speed = 5;
    private Vector3 startingPosition = new Vector3(0.0f, 1.5f, 0.0f);
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
        float xPosition = UnityEngine.Random.RandomRange(-1, 3);
        float zPosition = UnityEngine.Random.RandomRange(-1, 3);


        Vector3 position = new Vector3 (xPosition, 10, zPosition);

        Balloon.localPosition = transform.localPosition + position;

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
        //Not needed because of the ray sensors :D
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionTaken = actions.ContinuousActions;

        float actionDirection = actionTaken[0];
        float actionSteering = actionTaken[1];

        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime * actionDirection);
        transform.rotation = Quaternion.Euler(new Vector3(0, actionSteering * 180, 0));

        AddReward((actionDirection + actionSteering)*0.01f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> actions = actionsOut.ContinuousActions;

        actions[0] = 0;
        actions[1] = 0;

        if (Input.GetKey("w"))
        {
            actions[0] = 1;
        }
        if (Input.GetKey("s"))
        {
            actions[0] = -1;
        }
        if (Input.GetKey("d"))
            actions[1] = 0.10f;

        else if (Input.GetKey("a"))
            actions[1] = -0.10f;

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
        }

        if (collision.gameObject.CompareTag("Balloon")){
            AddReward(2.0f);
        }
    }

}
