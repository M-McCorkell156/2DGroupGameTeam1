using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio_Behaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip[] clips;
    public AudioSource source;
 
    public void PlayClip(int i)
    {
        source.clip = clips[i];
        source.enabled = true;
    }

    public void EndClip()
    { 
        source.enabled = false;
    }

}
