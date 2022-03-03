using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarratorTrigger : MonoBehaviour
{
    public SOUND_ID SoundId;

    public void PlaySound()
    {
        SoundManager.Instance.PlaySound(SoundId);
    }
}
