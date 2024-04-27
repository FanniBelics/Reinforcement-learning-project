using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class BlueyController : Agent
{
    public float speed = 5;
    private GameObject balloon;
    public Transform targetTransform;

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void OnEpisodeBegin()
    {
        float xPosition = UnityEngine.Random.RandomRange(-3, 11);
        float zPosition = UnityEngine.Random.RandomRange(-7, 10);

        targetTransform.localPosition = new Vector3(xPosition, 10, zPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
        }
        if (Input.GetKey("d"))
            transform.Rotate(0.0f, +0.5f, 0.0f);

        if (Input.GetKey("a"))
            transform.Rotate(0.0f, -0.5f, 0.0f);
    }
}
