using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Subtitles : MonoBehaviour
{
    public string text1;
    public string text2;
    public Text textBox;
    private AudioSource sound;

    public void ShowSubtitles()
    {
        int index = (int)(GetComponentInParent<NarratorTrigger>().SoundId);
        sound = SoundManager.Instance.audios[index - 1];
        StartCoroutine(SubsOrder());
    }
    IEnumerator SubsOrder()
    {
        textBox.GetComponentInParent<RawImage>().enabled = true;
        textBox.text = text1;
        yield return new WaitForSeconds(sound.clip.length / 2);
       
        textBox.text = text2;
        yield return new WaitForSeconds(sound.clip.length / 2);
        textBox.text = "";
        textBox.GetComponentInParent<RawImage>().enabled = false;
    }
}
