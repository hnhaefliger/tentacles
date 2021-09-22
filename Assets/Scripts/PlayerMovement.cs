using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tentacle;

public class PlayerMovement : MonoBehaviour
{
    public int nTentacles = 3;
    public float tentacleLength = 5f;

    public Transform cameraTransform;

    Rigidbody rb;
    Tentacle[] tentacles;

    void Start() {
        rb = GetComponent<Rigidbody>();

        tentacles = new Tentacle[nTentacles];

        for (int i = 0; i < nTentacles; i++)
        {
            tentacles[i] = new Tentacle(tentacleLength, rb);
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            if (tentacles[0].Shoot(cameraTransform.forward, cameraTransform.position))
            {
                Tentacle[] newTentacles = new Tentacle[nTentacles];
                newTentacles[nTentacles-1] = tentacles[0];

                for (int i = 1; i < nTentacles; i++)
                {
                    newTentacles[i-1] = tentacles[i];
                }

                tentacles = newTentacles;
            }
        }

        for (int i = 0; i < nTentacles; i++)
        {
            tentacles[i].Update(cameraTransform.position);
        }
    }
}
