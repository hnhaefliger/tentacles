using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public float maxTentacleLength = 10f;
    public Transform cameraTransform;
    public Transform playerTransform;

    Transform tentacle;

    private bool inUse = false;
    private bool teathered = false;
    private Vector3 velocity;

    void Start()
    {
        tentacle = GetComponent<Transform>();
    }

    void Update()
    {
        if (!inUse)
        {
            tentacle.position = playerTransform.position;
        }

        if (Input.GetMouseButtonDown(0))
        {
            inUse = true;
            velocity = cameraTransform.forward * 2f;
        }
        else
        {
            if (teathered)
            {
                // move player
            }
            else
            {
                tentacle.position += velocity * Time.deltaTime;

                if (Vector3.Distance(playerTransform.position, tentacle.position) > maxTentacleLength)
                {
                    velocity = new Vector3(0, 0, 0);
                    inUse = false;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        teathered = true;
        velocity = new Vector3(0, 0, 0);
    }
}
