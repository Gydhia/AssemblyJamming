using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tasks { None, PressButton, PlayGame }
public enum ObjectType { None, Chest, FinalDoor }

[RequireComponent(typeof(BoxCollider))]
public class ActivableItem : MonoBehaviour
{
    private Animator animator;
    public Tasks task;
    public ObjectType type;

    public Item requiredItem;
    public GameObject mesh;
    private BoxCollider sc;
    public BoxCollider Sc => sc;

    private bool hasBeenActivated = false;
    public bool monoActivation = true;

    // Start is called before the first frame update
    void Awake()
    {
        sc = GetComponent<BoxCollider>();
        mesh = GameObject.Instantiate(mesh, transform);
    }
    

    public void Execute()
    {
        if (!canExecute()) return;

        animator = mesh.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("trigger", true);
            SoundEffect();
        }
            
       
        
        switch (task)
        {
            case Tasks.None:
                break;
            case Tasks.PressButton:
                UseButton();
                break;
            case Tasks.PlayGame:
                Play2DGame();
                break;
        }

        if(monoActivation) sc.enabled = false;
        hasBeenActivated = true;
    }

    public void UseButton()
    {
        SoundManager.Instance.PlaySound(SOUND_ID.ChestOpen2D);
        GameManager.Instance.UnlockOne();
    }
    public void Play2DGame()
    {
        GameManager.Instance.ChangePlayer(true);
    }
    public bool canExecute()
    {
        if (this.hasBeenActivated && this.monoActivation) return false;
        
        if (transform.parent != null)
        {
            ActivableItem tmp = transform.parent.GetComponent<ActivableItem>();
            if (tmp != null)
            {
                if (!tmp.hasBeenActivated)
                {
                    return false;
                }
            }
        }
        
        if (requiredItem == null) return true;

        foreach (Item i in GameManager.Instance.items) {
            if (i == requiredItem) {
                GameManager.Instance.RemoveItem(i);
                return true;
            }
        }
        
        return false;
    }
    private void SoundEffect()
    {
        switch (type)
        {
            case ObjectType.Chest:
                StartCoroutine(waiter());
                break;
            case ObjectType.FinalDoor:
                SoundManager.Instance.PlaySound(SOUND_ID.SpaceDoorOpen);
                break;
        }
    }
    IEnumerator waiter()
    {
        SoundManager.Instance.PlaySound(SOUND_ID.OpenMetalChest);
        yield return new WaitForSeconds(0.9f);
        SoundManager.Instance.PlaySound(SOUND_ID.Confetti);
    }
}
