//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using System;
using UnityEngine;

public class ServingCounter : Counter
{
    // ---data members---
    public const int CUSTOMER_PENALTY = 50;
    public const float ANGRY_MULTIPLIER = 2f;
    public const float NONPLUSSED_THRESHOLD = 0.7f;

    [SerializeField] private GameObject customer;
    [SerializeField] private Transform patienceBar;
    [SerializeField] private Animator customerAnim;
    [SerializeField] private DishTag orderTag;

    private StageController stageController;
    private Item.ingredients[] order;
    private bool customerIsPresent;
    private bool customerIsNonPlussed;
    private bool customerIsAngry;
    private int playerToBlame;
    private float patienceStart;

    // these data member will be accessed directly, as they will be accessed at least every fixed update
    private float patienceCurrent;
    private float patienceCurrentPercent;

    // ---getters---
    private StageController GetStageController() { return stageController; }
    private Item.ingredients[] GetOrder() { return order; }
    private DishTag GetOrderTag() { return orderTag; }
    private GameObject GetCustomer() { return customer; }
    private Transform GetPatienceBar() { return patienceBar; }
    public bool GetCustomerIsPresent() { return customerIsPresent; }
    public bool GetCustomerIsNonPlussed() { return customerIsNonPlussed; }
    public bool GetCustomerIsAngry() { return customerIsAngry; }
    public int GetPlayerToBlame() { return playerToBlame; }
    public float GetPatienceStart() { return patienceStart; }

    // ---setters---
    private void SetStageController(StageController stageCont) { stageController = stageCont; }
    private void SetCustomerIsPresent(bool newValue) { customerIsPresent = newValue; }
    private void SetCustomerIsNonPlussed(bool newValue) { customerIsNonPlussed = newValue; }
    private void SetCustomerIsAngry(bool newValue) { customerIsAngry = newValue; }
    private void SetPlayerToBlame(int newValue) { playerToBlame = newValue; }
    private void SetPatienceCurrent(float newValue) { patienceCurrent = newValue; }
    private void SetPatienceStart(float newValue) { patienceStart = newValue; }

    // ---unity methods---
    private void Awake()
    {
        SetStageController(GameObject.FindGameObjectWithTag("Stage Controller").GetComponent<StageController>());
        order = new Item.ingredients[ItemStats.MAX_COMBO];
    }

    private void Update()
    {
        // manage customer timers and attitude
        if (customerIsPresent)
        {
            if (GetCustomerIsAngry())
            {
                patienceCurrent -= Time.deltaTime * ANGRY_MULTIPLIER;
            }
            else
            {
                patienceCurrent -= Time.deltaTime;
            }

            patienceCurrentPercent = patienceCurrent / patienceStart;
            patienceBar.transform.localScale = new Vector3(patienceCurrentPercent, 1.0f, 1.0f);

            if (!customerIsNonPlussed)
            {
                if (patienceCurrentPercent < NONPLUSSED_THRESHOLD)
                {
                    customerIsNonPlussed = true;
                    customerAnim.SetBool("NonPlussed", true);
                }
            }

            if (patienceCurrent <= 0.0f)
            {
                PatronOutOfPatience();
                patienceCurrent = 0;
                patienceCurrentPercent = 1;
            }
        }
    }

    // ---primary methods---

    // this version of receiveItem will compare the placed item with the customer oder. rewarding if correct, and penalizing if not.
    public override void receiveItem(Item item)
    {
        int playerNumber = item.GetHoldingPlayer().GetPlayerNumber();
        item.OnPlace(this);
        bool correctDish = CompareDishToOrder(item);
        // if the dish is correct...
        if (correctDish)
        {
            // ... and the customer is still about the patience threshold spawn a power up
            if (!GetCustomerIsNonPlussed())
            {
                stageController.SpawnPowerUp(playerNumber);
            }
            // add the score to the player that served them
            stageController.AddScore(playerNumber, item.GetScore());
            PatronLeave();
        }
        // if the order is wrong...
        else
        {
            // ... and the customer is already angry...
            if(GetCustomerIsAngry())
            {
                // ... and the player serving is different from the one who got the customer mad...
                if (GetPlayerToBlame() != playerNumber)
                {
                    // ... then he is now mad at both players
                    SetPlayerToBlame(0);
                }
            }
            // ... then is customer is mad now and blames the player who severed him
            else
            {
                SetCustomerIsAngry(true);
                customerAnim.SetBool("Mad", true);
                SetPlayerToBlame(playerNumber);
            }
        }

        item.DestroyItem();
    }

    // Brings a customer to the counter who makes and order and waits a number of seconds equal to their patience
    public void SummonPatorn(float patience)
    {
        SetPatienceCurrent(patience);
        SetPatienceStart(patience);
        SetPlayerToBlame(0);
        GetCustomer().SetActive(true);
        GetOrderTag().gameObject.SetActive(true);
        SetCustomerIsPresent(true);
        SetCustomerIsAngry(false);
        SetCustomerIsNonPlussed(false);
        int maxIngredients = Enum.GetNames(typeof(Item.ingredients)).Length;
        bool endIngredientList = false;
        for (int i = 0; i < ItemStats.MAX_COMBO; i++)
        {
            if (i == 0)
            {
                GetOrder()[i] = (Item.ingredients) (UnityEngine.Random.Range(1, maxIngredients));
            }
            else
            {
                if (!endIngredientList)
                {
                    int roll = (UnityEngine.Random.Range(0, maxIngredients));
                    GetOrder()[i] = (Item.ingredients)(roll);
                    if (roll == 0)
                    {
                        endIngredientList = true;
                    }
                }
                else
                {
                    GetOrder()[i] = Item.ingredients.empty;
                }
            }
        }
        GetOrderTag().ChangeTag(GetOrder(), true);
    }

    // Customer leaves and the player he is most angry with gets a score penalty
    private void PatronOutOfPatience()
    {
        if (GetCustomerIsAngry())
        {
            switch (GetPlayerToBlame())
            {
                case 0:
                    stageController.SubtractScore(1, CUSTOMER_PENALTY * 2);
                    stageController.SubtractScore(2, CUSTOMER_PENALTY * 2);
                    break;
                case 1:
                    stageController.SubtractScore(1, CUSTOMER_PENALTY * 2);
                    break;
                case 2:
                    stageController.SubtractScore(2, CUSTOMER_PENALTY * 2);
                    break;
            }
        }
        else
        {
            stageController.SubtractScore(1, CUSTOMER_PENALTY);
            stageController.SubtractScore(2, CUSTOMER_PENALTY);
        }

        PatronLeave();
    }

    // called to make the patron leave the counter and reset the variables
    private void PatronLeave()
    {
        SetCustomerIsPresent(false);
        GetOrderTag().gameObject.SetActive(false);
        GetCustomer().SetActive(false);
        for (int i = 0; i < GetOrder().Length; i++)
        {
            GetOrder()[i] = Item.ingredients.empty;
        }
        GetOrderTag().ChangeTag(GetOrder(), true);
        SetCustomerIsAngry(false);
        SetCustomerIsNonPlussed(false);
        customerAnim.SetBool("NonPlussed", false);
        customerAnim.SetBool("Mad", false);
    }

    // compare order to the dish served
    private bool CompareDishToOrder(Item item)
    {
        bool result = true;
        ComboItem dish = item.gameObject.GetComponent<ComboItem>();
        if (dish != null)
        {
            if (dish.GetPlate())
            {
                Item.ingredients[] dishIngredients = dish.GetContents();
                Item.ingredients[] orderIngredients = GetOrder();
                int[] dishTally = new int[Enum.GetNames(typeof(Item.ingredients)).Length];
                int[] orderTally = new int[Enum.GetNames(typeof(Item.ingredients)).Length];
                for (int i = 0; i < dishIngredients.Length; i++)
                {
                    dishTally[(int)dishIngredients[i]]++;
                }
                for (int i = 0; i < orderIngredients.Length; i++)
                {
                    orderTally[(int)orderIngredients[i]]++;
                }
                for (int i = 0; i < dishTally.Length; i++)
                {
                    if (dishTally[i] != orderTally[i])
                    {
                        result = false; // the ingredients don't match!
                        break;
                    }
                }
            }
            else
            {
                result = false; // no plate! how rude to just throw salad in his face!
            }
        }
        else
        {
            result = false; // this item is not a dish! 
        }

        return result;
    }
}
