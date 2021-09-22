using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1.0F;

    public Transform cameraTransform ;
    private Vector3 moveDirection = Vector3.zero;

    CharacterController controller;

    void Start() {
        controller = GetComponent<CharacterController>();
    }

    void Update() {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = cameraTransform.TransformDirection(moveDirection);
        moveDirection *= speed;

        controller.Move(moveDirection * Time.deltaTime);
    }
}
