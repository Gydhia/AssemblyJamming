using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DigicodeManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    private List<int> numbers;

    public static DigicodeManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        numbers = new List<int> { -1, -1, -1, -1 };
    }

    public void EnterredNumber(int nb)
    {
        SoundManager.Instance.PlaySound(SOUND_ID.Digicode);

        if(numbers[3] != -1)
        {
            for (int i = 0; i < 4; i++)
            {
                numbers[i] = -1;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (numbers[i] == -1)    {
                numbers[i] = nb;
                break;
            }
        }
        text.text = "";
        for (int i = 0; i < 4; i++)
        {
            if (numbers[i] == -1) text.text += ".";
            else text.text += numbers[i];
        }

        if (numbers[3] != -1) HasCode();
    }
    private void HasCode()
    {
        int index = 0, count = 0;
        foreach (Item item in GameManager.Instance.items)
        {
            if (item.type == ItemType.PaperSheet) {
                if ((int)(((PaperSheet)item).code) == numbers[index])
                {
                    count++;
                }
                index++;
            }
            
        }
        if (count == 4)
        {
            List<Item> tmp = new List<Item>();
            foreach(Item it in GameManager.Instance.items)
            {
                tmp.Add(it);
            }
            int ind = 0;
            foreach (Item item in tmp)
            {
                if (item.type == ItemType.PaperSheet)
                {
                    GameManager.Instance.RemoveItem(GameManager.Instance.items[ind]);
                    ind--;
                }
                ind++;
            }

            GameManager.Instance.UnlockOne();
            ParticleSystem ps = transform.parent.GetComponentInChildren<ParticleSystem>(true);
            ps.gameObject.SetActive(true);
            Destroy(transform.gameObject);
        }
    }
}
