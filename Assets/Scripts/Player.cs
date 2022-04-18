//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using UnityEngine;

// script for controllable player characters
//      it includes movement and interactions
public class Player : MonoBehaviour
{
    // ---data members---
    private const int SOUND_CLIP_2_CHANCE = 15;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private ItemInteractor itemInteractor;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource sounder;
    [SerializeField] private AudioClip[] chopSounds;
    [SerializeField] private Transform leftHandSlot;
    [SerializeField] private Transform rightHandSlot;
    [SerializeField] [Range(1,2)] private int playerNumber = 1;

    private StageController stageController;
    private HudController hudController;
    private Vector3 moveInput;
    private Quaternion toRotation;
    private Item leftHandItem;
    private Item rightHandItem;

    // these data member will be accessed directly, as they will be accessed at least every fixed update
    private bool controllable = true;
    private bool moving;
    private bool chopping;
    private float speedBoost;
    private float vert;
    private float hori;
    private float soundEffectNextPlayTime;
    [SerializeField] private float soundEffectDelay; // ---------------------------------- serialized for debug <--- make consts for default speeds once a default speed has been chosen
    [SerializeField] private float moveSpeed;        // ---------------------------------- serialized for debug <--- make consts for default speeds once a default speed has been chosen
    [SerializeField] private float rotationSpeed;    // ---------------------------------- serialized for debug <--- make consts for default speeds once a default speed has been chosen
    [SerializeField] private float choppingSpeed;    // ---------------------------------- serialized for debug <--- make consts for default speeds once a default speed has been chosen
    private Chopable choppingItem;

    // ---getters---
    public HudController GetHudController() { return hudController; }
    public Transform GetLeftHandSlot() { return leftHandSlot; }
    public Transform GetRightHandSlot() { return rightHandSlot; }
    public Item GetLeftHandItem() { return leftHandItem; }
    public Item GetRightHandItem() { return rightHandItem; }
    public int GetPlayerNumber() { return playerNumber; }

    // ---setters---
    public void SetLeftHandItem(Item newItem) { leftHandItem = newItem; }
    public void SetRightHandItem(Item newItem) { rightHandItem = newItem; }

    private void SetHudController(HudController hud) { hudController = hud; }
    private void SetStageController(StageController stageCont) { stageController = stageCont; }
    // ---unity methods---

    private void Update()
    {
        // reset movement detection
        moving = false;

        // read inputs for player 1
        if(controllable && playerNumber == 1)
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
        else if (controllable && playerNumber == 2)
        {
            if (Input.GetButton("LeftP2"))
            {
                moving = true;
                hori = -1;
            }
            else if (Input.GetButton("RightP2"))
            {
                moving = true;
                hori = 1;
            }
            else
            {
                hori = 0;
            }

            if (Input.GetButton("UpP2"))
            {
                moving = true;
                vert = 1;
            }
            else if (Input.GetButton("DownP2"))
            {
                moving = true;
                vert = -1;
            }
            else
            {
                vert = 0;
            }

            if (Input.GetButtonDown("PickUpP2"))
            {
                PickUpCommand();
            }

            if (Input.GetButtonDown("DropP2"))
            {
                DropCommand();
            }

            if (Input.GetButtonDown("UseP2"))
            {
                UseCommand();
            }
        }

        // set movement vector
        moveInput = new Vector3(hori * moveSpeed * speedBoost, 0.0f, vert * moveSpeed);
        characterController.SimpleMove(moveInput);

        // rotate character
        if (moving)
        {
            toRotation = Quaternion.LookRotation(moveInput, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * speedBoost * Time.deltaTime);
        }

    }

    private void FixedUpdate()
    {
        // handle chopping loop. leave the loop after chopping time hits 0 and chopping item is set to null
        if(chopping)
        {
            if (soundEffectNextPlayTime < Time.time)
            {
                if (Random.Range(1, 101) < SOUND_CLIP_2_CHANCE)
                {
                    sounder.clip = chopSounds[1];
                }
                else
                {
                    sounder.clip = chopSounds[0];
                }
                sounder.Play();
                soundEffectNextPlayTime = Time.time + soundEffectDelay;
            }
            if (choppingItem != null)
            {
                float chopTimeLeft = choppingItem.GetChoppingTimeLeft();
                if (chopTimeLeft > 0)
                {
                    choppingItem.SetChoppingTimeLeft(chopTimeLeft - (choppingSpeed * speedBoost));
                }
                else
                {
                    choppingItem.ReplaceWithCombo();
                    choppingItem = null;
                }
            }
            else
            {
                StopChop();
            }
        }
    }

    // ---primary methods---

    // used instead of "awake"
    public void Initialize(StageController stageCont, HudController hudCont)
    {
        // Initialize null variables
        SetStageController(stageCont);
        SetHudController(hudCont);
        speedBoost = 1;
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
            hudController.NoticeFullHands(this);
            return false;
        }
    }

    // call if player wants to attempt to pick up an item
    private void PickUpCommand()
    {
        if (HasOpenHand())
        {
            Appliance appliance = itemInteractor.GetAppliance();
            if (appliance != null)
            {
                // if the player is picking up from a counter then get the item on it, if it has one
                Counter counter = appliance.gameObject.GetComponent<Counter>();
                if (counter != null)
                {
                    Item itemOnCounter = counter.GetItemOnCounter();
                    if (itemOnCounter != null)
                    {
                        // check if the item is a power up
                        PowerUp powerUp = itemOnCounter.gameObject.GetComponent<PowerUp>();
                        if (powerUp != null)
                        {
                            if (powerUp.GetPlayerNumber() == playerNumber)
                            {
                                ActivatePowerUp(powerUp);
                                counter.RemoveItem();
                            }
                        }
                        else
                        {
                            PickUpItem(itemOnCounter);
                            counter.RemoveItem();
                        }
                        return;
                    }
                }
                // if the player uses the pickUp command on a crate it should be treated the same
                //      as if they had used the "use" command on it. (special case for better UI)
                Crate crate = appliance.gameObject.GetComponent<Crate>();
                if (crate != null)
                {
                    useAppliance(appliance);
                    return;
                }
            }
        }
    }

    // call if player wants to attempt to drop an item
    private void DropCommand()
    {
        Item leftHandItem = GetLeftHandItem();
        if (leftHandItem != null)
        {
            Appliance appliance = itemInteractor.GetAppliance();
            if (appliance != null)
            {
                Counter counter = appliance.gameObject.GetComponent<Counter>();
                ServingCounter servingCounter = appliance.gameObject.GetComponent<ServingCounter>();
                if (servingCounter != null)
                {
                    if (servingCounter.GetCustomerIsPresent())
                    {
                        PlaceItem(servingCounter, leftHandItem);
                    }
                }
                else if (counter != null)
                {
                    Item itemOnCounter = counter.GetItemOnCounter();
                    if (itemOnCounter == null)
                    {
                        PlaceItem(counter, leftHandItem);
                    }
                    else if (itemOnCounter.GetType().Equals(typeof(ComboItem)) && leftHandItem.GetType().Equals(typeof(ComboItem)))
                    {
                        CombineItems(counter, leftHandItem);
                    }
                }
            }
        }
    }

    // call if player wants to attempt to use an appliance
    private void UseCommand()
    {
        if (HasOpenHand())
        {
            Appliance appliance = itemInteractor.GetAppliance();
            if (appliance !=null)
            {
                useAppliance(appliance);
            }
        }
    }

    // use an appliance
    public void useAppliance(Appliance appliance)
    {
        appliance.Use(this);
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
        item.OnPickUp(this);
        item.transform.parent = openHand;
        item.transform.localPosition = Vector3.zero;
        UpdateInventoryHud();
    }

    // activate power-up
    public void ActivatePowerUp(PowerUp powerUp)
    {
        powerUp.Activate();
    }

    // place/drop an item
    public void PlaceItem(Counter destination, Item itemInhand)
    {
        // place the item on a counter
        itemInhand.transform.SetParent(null);
        SetLeftHandItem(null);
        destination.receiveItem(itemInhand);
        SwapItemRightToLeftHand();
        UpdateInventoryHud();
    }

    // combine items
    public void CombineItems(Counter destination, Item itemInhand)
    {
        // combine counter combine item with the one being held
        ComboItem destinationCombo = (ComboItem)destination.GetItemOnCounter();
        ComboItem incomingCombo = (ComboItem)itemInhand;
        destinationCombo.Combine(incomingCombo);
        UpdateInventoryHud();
    }

    // destroy item in players left hand
    public void DestroyItemInLeftHand()
    {
        GetLeftHandItem().DestroyItem();
        SetLeftHandItem(null);
        SwapItemRightToLeftHand();
        UpdateInventoryHud();
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

    // start chopping
    public void StartChop(Chopable chopItem)
    {
        controllable = false;
        chopping = true;
        animator.SetBool("chopping", true);
        choppingItem = chopItem;
    }

    // stop the chop!
    public void StopChop()
    {
        animator.SetBool("chopping", false);
        chopping = false;
        controllable = true;
    }

    // Start Speed Boost
    public void StartSpeedBoost(float boostFactor)
    {
        speedBoost = boostFactor;
    }

    // Start Speed Boost
    public void StopSpeedBoost()
    {
        speedBoost = 1;
    }

    // Updates the hud with the items currently in hand
    public void UpdateInventoryHud()
    {
        GetHudController().UpdateInventoryHud(playerNumber, leftHandItem, rightHandItem);
    }
}
