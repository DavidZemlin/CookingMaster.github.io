using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for controllable player characters
//      it includes movement and interactions
public class Player : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] [Range(1,2)] private int playerNumber = 1;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotationSpeed;

    private Vector3 moveInput;
    private Quaternion toRotation;
    private float vert;
    private float hori;
    private bool moving; // this can be used to signal animation transitions

    private void Awake()
    {
        // Initialize characterController
        if (characterController == null)
        {
            characterController = gameObject.GetComponent<CharacterController>();
        }
    }

    private void Update()
    {
        // reset movement detection
        moving = false;

        // read inputs for player 1
        if(playerNumber == 1)
        {
            if (Input.GetButton("LeftP1"))
            {
                moving = true;
                hori = -1;
            }
            else if (Input.GetButton("RightP1"))
            {
                moving = true;
                hori = 1;
            }
            else
            {
                hori = 0;
            }

            if (Input.GetButton("UpP1"))
            {
                moving = true;
                vert = 1;
            }
            else if (Input.GetButton("DownP1"))
            {
                moving = true;
                vert = -1;
            }
            else
            {
                vert = 0;
            }
        }
        else if (playerNumber == 2)
        {
            // copy all player one controlls here when done-----------------------------------------------------------------------------------------
        }

        // set movement vector
        moveInput = new Vector3(hori * maxSpeed, 0.0f, vert * maxSpeed);
        characterController.SimpleMove(moveInput);

        // rotate character
        if (moving)
        {
            toRotation = Quaternion.LookRotation(moveInput, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

    }
}
