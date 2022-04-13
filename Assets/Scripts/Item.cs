//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// base class for items that can be picked up by the player
public abstract class Item : MonoBehaviour
{
    // ---data members---
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

    [SerializeField] private Rigidbody rBody;       // currently the rigidbody and collider are not used; here for future implementation
    [SerializeField] private Collider itemCollider; // currently the rigidbody and collider are not used; here for future implementation

    private Transform itemTransform;
    private Vector3 initialScale;

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
        itemTransform = transform;
    }

    // ---primary methods---

    // should be called whenever this item is picked up
    public void OnPickUp()
    {
        rBody.isKinematic = true;
        rBody.useGravity = false;
        itemCollider.enabled = false;
        itemTransform.localEulerAngles = Vector3.zero;
    }

    // call this when placing the item in/on something
    public void OnPlace()
    {
        itemTransform.localEulerAngles = Vector3.zero;
    }
}
