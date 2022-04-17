//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
// ---------------------------------- serialized for debug
// ---data members---
// ---getters---
// ---setters---
// ---constructors---
// ---unity methods---
// ---primary methods---
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingCounter : Counter
{
    // ---data members---
    public const int CUSTOMER_PENALTY = 100;
    public const float ANGRY_MULTIPLIER = 1.5f;
    public const float NONPLUSSED_THRESHOLD = 0.7f;

    [SerializeField] private GameObject customer;
    [SerializeField] private Transform patienceBar;
    [SerializeField] private Animator customerAnim;
    [SerializeField] private DishTag orderTag;

    private StageController stageController;
    [SerializeField] private Item.ingredients[] order;
    private bool customerIsPresent;
    private bool customerIsNonPlussed;
    private bool customerIsAngry;
    private int playerToBlame;
    private float patienceStart;

    // these data member will be accessed directly, as they will be accessed at least every fixed update
    [SerializeField] private float patienceCurrent; // ---------------------------------- serialized for debug
    [SerializeField] private float patienceCurrentPercent; // ---------------------------------- serialized for debug

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

    // this version of receiveItem will destroy the item and deduct score from the player who threw it out
    public override void receiveItem(Item item)
    {
        item.OnPlace(this);
        item.DestroyItem();
    }

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
}
