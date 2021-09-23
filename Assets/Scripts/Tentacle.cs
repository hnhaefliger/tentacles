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

    public Tentacle(float tentacleLength, Rigidbody playerRb, Material material)
    {
        meshObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        meshObject.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        meshObject.GetComponent<MeshRenderer>().material = material;

        transform = meshObject.GetComponent<Transform>();
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
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

        transform.LookAt(playerPosition - new Vector3(0f, 0.4f, 0f));
        transform.Rotate(Vector3.right, 90);
        transform.localScale = new Vector3(0.1f, Vector3.Distance(transform.position, playerPosition), 0.1f);
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
