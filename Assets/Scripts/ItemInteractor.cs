using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractor : MonoBehaviour
{
    [SerializeField] private Player player;



    private void Awake()
    {
        if (player == null)
        {
            player = GetComponentInParent<Player>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I am touching: " + other.gameObject.name);
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}
