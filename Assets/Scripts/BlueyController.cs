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
    public GameObject balloonPrefab;
    private Vector3 startingPosition = new Vector3(0.0f, 1.5f, -8.0f);
    public Rigidbody rb;
    private GameObject newBalloon;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start...");
    }

    public override void OnEpisodeBegin()
    {
        GameObject[] existingBalloons = GameObject.FindGameObjectsWithTag("Balloon");
        if(existingBalloons.Length > 0 )
        {
            foreach( GameObject balloon in existingBalloons)
            {
                Destroy(balloon);
            }
        }

        Debug.Log("Episode begins");
        float xPosition = UnityEngine.Random.RandomRange(-3, 11);
        float zPosition = UnityEngine.Random.RandomRange(-7, 10);

        newBalloon = Instantiate(balloonPrefab, new Vector3(xPosition, 10, zPosition), Quaternion.identity);
        transform.localPosition = startingPosition;
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

        AddReward((actionDirection + actionSteering));
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
  

    public void EndThisEpisode()
    {
        AddReward(-10);
        Destroy(newBalloon);
        EndEpisode();
    }


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
            AddReward(0.5f);
        }
    }

}
