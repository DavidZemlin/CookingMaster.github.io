using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// base class for items that can be picked up by the player
public abstract class Item : MonoBehaviour
{
    [SerializeField] private Rigidbody rBody;
    [SerializeField] private Collider itemCollider;
    [SerializeField] private Vector3 placedFacing;
    private Transform itemTransform;

    // initialized any variable that are null
    private void Awake()
    {
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

    // call this whenever it is picked up
    public void PickupItem()
    {
        rBody.isKinematic = true;
        itemCollider.enabled = false;
    }

    // call this whenever it is dropped (not placed)
    public void DropItem()
    {
        rBody.isKinematic = false;
        itemCollider.enabled = true;
    }

    // call this when placing the item in/on something
    public void PlaceItem()
    {
        itemTransform.localEulerAngles = placedFacing;
    }
}
