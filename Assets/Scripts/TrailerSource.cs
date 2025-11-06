using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerSource : MonoBehaviour
{
    public Material[] materials;
    public TrailRenderer trailRenderer;

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void changeColor(int bounceTimes)
    {
        trailRenderer.material = materials[bounceTimes];
    }
}
