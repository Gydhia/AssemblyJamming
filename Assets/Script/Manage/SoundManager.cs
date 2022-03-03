using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SOUND_ID
{
    WorldTheme3D = 1,
    WorldTheme2D = 2,
    WorldTheme2DFiltered = 3,
    ChestOpen2D = 4,
    CloseMetalChest = 5,
    ComputerFan = 6,
    Confetti = 7,
    Digicode = 8, // 5 iterations
    DoubleJump = 13,
    EnemyHitsPlayer = 14,
    Entering2D = 15,
    Exiting2D = 16,
    PickUpItem = 17,
    Jump = 18,
    OpenMetalChest = 19,
    PlayerDeath = 20,
    PlayerHitsEnemy = 21,
    SpaceDoorOpen = 22,
    Steps3D = 23, // 2 iterations
    Steps2D = 25, // 2 iteratios
    SwordSwing = 27,
    UnlockLock = 28,

    Beginning = 29,
    UnlockDoubleJump = 30,
    UnlockSword = 31,
    PaperSheet = 32
}

public class SoundManager : MonoBehaviour
{
    public AudioSource[] audios;
    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }

        foreach(AudioSource audio in audios)
        {
            audio.Stop();
        }
    }

    public void PlaySound(SOUND_ID id)
    {
        audios[(int)SOUND_ID].Play();
    }
}
