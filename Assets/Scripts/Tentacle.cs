using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle
{
    GameObject meshObject;
    Transform transform;
    SpringJoint tentacle;
    float maxLength;
    bool teathered = false;

    public Tentacle(float tentacleLength, Rigidbody playerRb)
    {
        meshObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        meshObject.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        meshObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Diffuse"));

        transform = meshObject.GetComponent<Transform>();
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        maxLength = tentacleLength;

        tentacle = meshObject.AddComponent<SpringJoint>();
        tentacle.connectedBody = playerRb;
    }

    public void Update(Vector3 playerPosition)
    {
        if (!teathered)
        {
            transform.position = playerPosition;
        }
    }

    public bool Shoot(Vector3 direction, Vector3 playerPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(playerPosition, direction), out hit, maxLength))
        {
            transform.position = hit.point;
            teathered = true;
        }
        else
        {
            teathered = false;
        }

        return teathered;
    }
}
