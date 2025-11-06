using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Covers : TargetObject
{
    public GameObject closed;
    public GameObject opened;

    public override void activate()
    {

            closed.SetActive(false);
            opened.SetActive(true);
 
    }
}
