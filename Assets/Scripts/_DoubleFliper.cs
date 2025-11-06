using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _DoubleFliper : TargetObject
{
    public GameObject upperGate;
    public GameObject lowerGate;
    private int progression = 0;

    public GameObject audioPlayer;
    public AudioClip endGameAudio;

    public override void activate()
    {
        if (progression == 0)
        {
            lowerGate.SetActive(false);
            progression = 1;
        }
        else
        {
            upperGate.SetActive(false);
            audioPlayer.GetComponent<AudioSource>().clip = endGameAudio;
            audioPlayer.GetComponent<AudioSource>().Play();
        }
            
    }
}
