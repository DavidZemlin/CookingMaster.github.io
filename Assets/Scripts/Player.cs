using UnityEngine;

// script for controllable player characters
//      it includes movement and interactions
public class Player : MonoBehaviour
{
    [SerializeField] private HudController hudController;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private ItemInteractor itemInteractor;
    [SerializeField] private Transform leftHandSlot;
    [SerializeField] private Transform rightHandSlot;
    [SerializeField] [Range(1,2)] private int playerNumber = 1;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotationSpeed;

    private Vector3 moveInput;
    private Quaternion toRotation;
    private float vert;
    private float hori;
    private bool moving;
    private bool pickUp;
    private bool drop;
    private bool chop;

    [SerializeField] private Item leftHandItem; // --------------------------------------------------------------------------
    [SerializeField] private Item rightHandItem; // --------------------------------------------------------------------------

    // getters
    public Transform GetLeftHandSlot() { return leftHandSlot; }
    public Transform GetRightHandSlot() { return rightHandSlot; }
    public Item GetLeftHandItem() { return leftHandItem; }
    public Item GetRightHandItem() { return rightHandItem; }

    // setters
    public void SetLeftHandItem(Item newItem) { leftHandItem = newItem; }
    public void SetRightHandItem(Item newItem) { rightHandItem = newItem; }

    // Initialize null variables
    private void Awake()
    {
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

    public void useAppliance()
    {
        if (HasOpenHand())
        {
            Appliance appliance = itemInteractor.GetAppliance();
            if (appliance != null)
            {
                appliance.Use(this);
            }
        }
    }

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
            if (itemInteractor.getItem() != null)
            {
                PickUpItem(itemInteractor.getItem());
            }
        }
    }

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
            else
            {
                //DropItem(GetLeftHandItem());   //------------------------------------------------------disabled till for now ------------------
            }
        }
    }

    private void UseCommand()
    {
        if (HasOpenHand() && itemInteractor.GetAppliance() != null)
        {
            itemInteractor.GetAppliance().Use(this);
        }
    }

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

    public void PlaceItem(Counter destination, Item incommingItem)
    {
        if (destination.GetItemOnCounter() == null)
        {
            incommingItem.transform.SetParent(null);
            SetLeftHandItem(null);
            destination.recieveItem(incommingItem);
            SwapItemRightToLeftHand();
        }
        else if (destination.GetItemOnCounter().GetType().Equals(typeof(ComboItem)) && incommingItem.GetType().Equals(typeof(ComboItem)))
        {
            ComboItem destinationCombo = (ComboItem) destination.GetItemOnCounter();
            ComboItem incomingCombo = (ComboItem) incommingItem;
            destinationCombo.Combine(incomingCombo);
        }
    }

    public void DropItem(Item itemToDrop)
    {
        SetLeftHandItem(null);
        itemToDrop.OnDrop();
        SwapItemRightToLeftHand();
    }

    public void SwapItemRightToLeftHand()
    {
        if (rightHandItem != null && leftHandItem == null)
        {
            PickUpItem(rightHandItem);
            SetRightHandItem(null);
        }
    }
}
