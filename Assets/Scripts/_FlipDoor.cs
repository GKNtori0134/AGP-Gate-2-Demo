using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _FlipDoor : TargetObject
{
    public override void activate()
    {
        transform.position = new Vector3(9999,9999,9999);
    }
}
