using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{

    public TargetObject targetObject;
    public GameObject anotherButtonComp;
    public Material[] materials;
    public int bounceTimes = 2;
    bool triggered = false;

    public void triggerTarget()
    {
        if (triggered) return;
        targetObject.activate();
        GetComponent<MeshRenderer>().material = materials[bounceTimes];
        anotherButtonComp.GetComponent<MeshRenderer>().material = materials[bounceTimes];
        triggered = true;
    }
}
