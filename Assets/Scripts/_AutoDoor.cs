using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _AutoDoor : TargetObject
{
    private bool isOpening = false;

    private void Update()
    {
        if (isOpening) transform.position += new Vector3(0,10*Time.deltaTime,0);
    }

    public override void activate()
    {
        isOpening = true;
    }
}
