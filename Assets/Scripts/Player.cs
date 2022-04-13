//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
// ---------------------------------- serialized for debug
// ---data members---
// ---getters---
// ---setters---
// ---constructors---
// ---unity methods---
// ---primary methods---
using UnityEngine;

// script for controllable player characters
//      it includes movement and interactions
public class Player : MonoBehaviour
{
    // ---data members---
    [SerializeField] private CharacterController characterController;
    [SerializeField] private ItemInteractor itemInteractor;
    [SerializeField] private Transform leftHandSlot;
    [SerializeField] private Transform rightHandSlot;
    [SerializeField] [Range(1,2)] private int playerNumber = 1;

    private HudController hudController;
    private Vector3 moveInput;
    private Quaternion toRotation;
    [SerializeField] private Item leftHandItem; // ---------------------------------- serialized for debug
    [SerializeField] private Item rightHandItem; // ---------------------------------- serialized for debug
    [SerializeField] private float maxSpeed; // ---------------------------------- serialized for debug <--- make consts for default speeds
    [SerializeField] private float rotationSpeed; // ---------------------------------- serialized for debug <--- make consts for default speeds
    private float vert;
    private float hori;
    private bool moving;
    private bool pickUp;
    private bool drop;
    private bool chop;

    // ---getters---
    public Transform GetLeftHandSlot() { return leftHandSlot; }
    public Transform GetRightHandSlot() { return rightHandSlot; }
    public Item GetLeftHandItem() { return leftHandItem; }
    public Item GetRightHandItem() { return rightHandItem; }

    // ---setters---
    public void SetLeftHandItem(Item newItem) { leftHandItem = newItem; }
    public void SetRightHandItem(Item newItem) { rightHandItem = newItem; }

    // ---unity methods---
    private void Awake()
    {
        // Initialize null variables
        if (hudController == null)
        {
            hudController = GameObject.FindGameObjectWithTag("HUD").GetComponent<HudController>();
        }
        if (characterController == null)
        {
            characterController = gameObject.GetComponent<CharacterController>();
        }
        if (itemInteractor == null)
        {
            itemInteractor = gameObject.GetComponentInChildren<ItemInteractor>();
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

            if (Input.GetButtonDown("PickUpP1"))
            {
                PickUpCommand();
            }

            if (Input.GetButtonDown("DropP1"))
            {
                DropCommand();
            }

            if (Input.GetButtonDown("UseP1"))
            {
                UseCommand();
            }
        }
        else if (playerNumber == 2)
        {
            // copy all player one controls here when done-----------------------------------------------------------------------------------------
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

    // ---primary methods---

    // check if the player has an open hand
    public bool HasOpenHand()
    {
        if (GetRightHandItem() == null || GetLeftHandItem() == null)
        {
            return true;
        }
        else
        {
            hudController.FullHandsNotice();
            return false;
        }
    }

    // call player wants to attempt to pick up an item
    private void PickUpCommand()
    {
        if (HasOpenHand())
        {
            if (itemInteractor.GetAppliance() != null)
            {
                Counter counter = itemInteractor.GetAppliance().gameObject.GetComponent<Counter>();
                if (counter != null && counter.GetItemOnCounter() != null)
                {
                    PickUpItem(counter.GetItemOnCounter());
                    counter.RemoveItem();
                    return;
                }
            }
        }
    }

    // call player wants to attempt to drop an item
    private void DropCommand()
    {
        if (GetLeftHandItem() != null)
        {
            if (itemInteractor.GetAppliance() != null)
            {
                Counter counter = itemInteractor.GetAppliance().gameObject.GetComponent<Counter>();
                if (counter != null && counter.GetItemOnCounter() == null)
                {
                    PlaceItem(counter, GetLeftHandItem());
                    return;
                }
            }
        }
    }

    // call player wants to attempt to use an appliance
    private void UseCommand()
    {
        if (HasOpenHand() && itemInteractor.GetAppliance() != null)
        {
            useAppliance();
        }
    }

    // use an appliance
    public void useAppliance()
    {
        Appliance appliance = itemInteractor.GetAppliance();
        if (appliance != null)
        {
            appliance.Use(this);
        }
    }

    // pick up an item
    public void PickUpItem(Item item)
    {
        Transform openHand = null;
        if (GetLeftHandItem() == null)
        {
            openHand = GetLeftHandSlot();
            SetLeftHandItem(item);
        }
        else
        {
            openHand = GetRightHandSlot();
            SetRightHandItem(item);
        }
        item.OnPickUp();
        item.transform.parent = openHand;
        item.transform.localPosition = Vector3.zero;
    }

    // place/drop an item
    public void PlaceItem(Counter destination, Item itemInhand)
    {
        // if counter is empty, place the item
        if (destination.GetItemOnCounter() == null)
        {
            itemInhand.transform.SetParent(null);
            SetLeftHandItem(null);
            destination.recieveItem(itemInhand);
            SwapItemRightToLeftHand();
        }
        // if the counter has an item on it, attempt to combine the items if both items are combo items
        else if (destination.GetItemOnCounter().GetType().Equals(typeof(ComboItem)) && itemInhand.GetType().Equals(typeof(ComboItem)))
        {
            ComboItem destinationCombo = (ComboItem) destination.GetItemOnCounter();
            ComboItem incomingCombo = (ComboItem) itemInhand;
            destinationCombo.Combine(incomingCombo);
        }
    }

    // move an item from the right hand to the left hand
    public void SwapItemRightToLeftHand()
    {
        if (rightHandItem != null && leftHandItem == null)
        {
            PickUpItem(rightHandItem);
            SetRightHandItem(null);
        }
    }
}
