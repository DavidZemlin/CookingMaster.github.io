using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// base class for items that can be picked up by the player
public abstract class Item : MonoBehaviour
{
    public enum ingredients
    {
        berrie,
        carrots,
        lettuce,
        redCabbage,
        squash,
        tomato,
    }

    [SerializeField] private Rigidbody rBody;
    [SerializeField] private Collider itemCollider;
    [SerializeField] private Vector3 placedFacing;
    [SerializeField] private Vector3 heldFacing;
    private Transform itemTransform;
    private Vector3 initialScale;

    // initialized any variable that are null
    private void Awake()
    {
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

    // call this whenever it is picked up
    public void OnPickUp()
    {
        rBody.isKinematic = true;
        rBody.useGravity = false;
        itemCollider.enabled = false;
        itemTransform.localEulerAngles = Vector3.zero;
    }

    // call this whenever it is dropped (not placed)
    public void OnDrop()
    {
        transform.parent = null;
        transform.localScale = initialScale;
        rBody.isKinematic = false;
        rBody.useGravity = true;
        itemCollider.enabled = true;
    }

    // call this when placing the item in/on something
    public void OnPlace()
    {
        itemTransform.localEulerAngles = Vector3.zero;
    }
}
