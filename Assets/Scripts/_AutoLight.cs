using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _AutoLight : TargetObject
{
    public GameObject lights;

    public override void activate()
    {
        lights.SetActive(true);
    }
}
