using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject inventory;
    public List<Item> items = new List<Item>();

    public Text text;
    Scene scene;
    private float startTime;
    private float roundTime = 10f;
    public int timer = 0;

    public Camera cam;
    public bool in3D = true;
    
    public GameObject player;
    public GameObject player2D;
    
    public Item doorOpener;
    public int[] unlocked;

    public static GameManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);       
        }
        unlocked = new int[4] { 0, 0, 0, 0 };
    }

    public void Start()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.name == "3DScene")
        {
            SoundManager.Instance.PlaySound(SOUND_ID.WorldTheme3D);
            SoundManager.Instance.PlaySound(SOUND_ID.WorldTheme2DFiltered);
            SoundManager.Instance.PlaySound(SOUND_ID.WorldTheme2D);
            SoundManager.Instance.PlaySound(SOUND_ID.WorldTheme2D, false);
            SoundManager.Instance.PlaySound(SOUND_ID.Beginning);
        }

        startTime = Time.time;
    }

    private void Update()
    {
        if(unlocked[unlocked.Length - 1] == 0)
        {
            if (Time.time - startTime >= roundTime)
            {
                timer += 1;
            }
        }
    }
    public void UnlockOne()
    {
        for (int i = 0; i < unlocked.Length; i++){
            if (unlocked[i] == 0){
                unlocked[i] = 1;
                break;
            }
        }
        SoundManager.Instance.PlaySound(SOUND_ID.UnlockLock);
        if (CheckIfFinish()) AddItem(doorOpener);
    }
    public bool CheckIfFinish()
    {
        int nb = 0;
        for (int i = 0; i < unlocked.Length; i++)
        {
            if (unlocked[i] == 1) nb++;
        }
        return nb == unlocked.Length;
    }
    public void AddItem(Item item)
    {
        items.Add(item);
        AddToUI(item);

        if (item.type == ItemType.Sword)
        {
            player2D.GetComponent<PlayerController2D>().GetSword();
        }
        if (item.type == ItemType.CloudInABottle)
        {
            player2D.GetComponent<PlayerController2D>().GetCloud();
        }
    }
    public void RemoveItem(Item item)
    {
        RemoveFromUI(item);
        items.Remove(item);
    }
    public void AddToUI(Item item)
    {
        GameObject itemToAdd;
        itemToAdd = new GameObject(item.name);
        itemToAdd.AddComponent<Image>().preserveAspect = true;
        itemToAdd.GetComponent<Image>().sprite = item.icon;

        itemToAdd.transform.parent = inventory.transform;
    }
    public void RemoveFromUI(Item item)
    {
        GameObject tmp = inventory;
        int index = 0;
        foreach (Transform go in tmp.transform)
        {
            if (go.name == item.name) Destroy(inventory.transform.GetChild(index).gameObject);
            index++;
        }

    }
    public void ChangePlayer(bool isIn3D)
    {
        if (in3D) // When pressing E on the "Tabouret"
        {
            SoundManager.Instance.PlaySound(SOUND_ID.Entering2D);
            SoundManager.Instance.PlaySound(SOUND_ID.WorldTheme3D, false);
            SoundManager.Instance.PlaySound(SOUND_ID.WorldTheme2DFiltered, false);
            SoundManager.Instance.PlaySound(SOUND_ID.WorldTheme2D);

            cam.GetComponentInParent<PlayerMovement>().enabled = false;
            cam.GetComponent<MouseLook>().isIn3D = false;
            player2D.GetComponent<PlayerController2D>().UnlockMove();

            in3D = false;
            
        } 
        else // When escaping the 2D game
        {
            if(isIn3D == false)
            {
                SoundManager.Instance.PlaySound(SOUND_ID.Exiting2D);
                SoundManager.Instance.PlaySound(SOUND_ID.WorldTheme2D, false);
                SoundManager.Instance.PlaySound(SOUND_ID.WorldTheme2DFiltered);
                SoundManager.Instance.PlaySound(SOUND_ID.WorldTheme3D);

                cam.GetComponentInParent<PlayerMovement>().enabled = true;
                cam.GetComponent<MouseLook>().isIn3D = true;
                player2D.GetComponent<PlayerController2D>().LockMove();

                in3D = true;
            }
        }
    }
  
    public void LoadMenu()
    {
        
        SceneManager.LoadScene("Menu");
        scene = SceneManager.GetActiveScene();
    }
    public void LoadEndGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("EndScene");
        scene = SceneManager.GetActiveScene();

        //int minutes = timer / 60;
        //text.text = "You made it in [" + minutes + " min - " + timer % 60 + " sec] !";
    } 
}
