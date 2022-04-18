//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using UnityEngine;

// this base class is for use of appliances. anything from crates to counter-tops to stove tops etc...
public abstract class Appliance : MonoBehaviour
{
    // ---primary methods---

    // base use function that appliances will use to do their stuff
    public abstract void Use(Player usingPlayer);
}
