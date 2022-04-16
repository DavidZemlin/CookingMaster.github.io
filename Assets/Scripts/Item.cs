//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
// ---------------------------------- serialized for debug
// ---data members---
// ---getters---
// ---setters---
// ---constructors---
// ---unity methods---
// ---primary methods---
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// base class for items that can be picked up by the player
public abstract class Item : MonoBehaviour
{
    // ---Enums---
    public enum ingredients
    {
        empty, // used for combo slots that are empty
        berrie,
        carrots,
        lettuce,
        redCabbage,
        squash,
        tomato,
    }

    // ---data members---
    [SerializeField] private Rigidbody rBody;       // currently the rigidbody and collider are not used; here for future implementation
    [SerializeField] private Collider itemCollider; // currently the rigidbody and collider are not used; here for future implementation
    [SerializeField] private Counter currentCounter;

    private Vector3 initialScale;
    private Player holdingPlayer;
    private Player lastHoldingPlayer; //used for remembering who last had this item.
    private int score;

    // ---getters---
    public Counter GetCurrentCounter() { return currentCounter; }
    public Player GetHoldingPlayer() { return holdingPlayer; }
    public Player GetLastHoldingPlayer() { return lastHoldingPlayer; }
    public int GetScore() { return score; }

    // ---setters---
    private void SetCurrentCounter(Counter newCounter) { currentCounter = newCounter; }
    private void SetHoldingPlayer(Player newHoldingPlayer) { holdingPlayer = newHoldingPlayer; }
    private void SetLastHoldingPlayer(Player newLhp) { holdingPlayer = newLhp; }
    protected void SetScore(int newScore) { score = newScore; }

    // ---unity methods---
    private void Awake()
    {
        // initialized all variable
        initialScale = transform.localScale;
        if (rBody == null)
        {
            rBody = gameObject.GetComponent<Rigidbody>();
        }
        if (itemCollider == null)
        {
            itemCollider = gameObject.GetComponent<Collider>();
        }
    }

    // ---primary methods---

    // should be called whenever this item is picked up
    public void OnPickUp(Player player)
    {
        rBody.isKinematic = true;
        rBody.useGravity = false;
        itemCollider.enabled = false;
        SetCurrentCounter(null);
        transform.localEulerAngles = Vector3.zero;
        SetHoldingPlayer(player);
        lastHoldingPlayer = player;
    }

    // call this when placing the item in/on something
    public void OnPlace(Counter counter)
    {
        SetCurrentCounter(counter);
        transform.localEulerAngles = Vector3.zero;
        SetHoldingPlayer(null);
    }

    // destroys this item
    public void DestroyItem()
    {
        Counter counter = GetCurrentCounter();
        if (counter != null && currentCounter.GetItemOnCounter() == this)
        {
            counter.RemoveItem();
        }
        Destroy(gameObject);
    }
}
