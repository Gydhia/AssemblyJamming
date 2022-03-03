using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [SerializeField]
    private bool _isOpen = false;

    public ButtonScript button1;
    public ButtonScript button2;
    public ButtonScript button3;
    public ButtonScript button4;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetBool("open", false);
    }

    // Update is called once per frame
    void Update()
    {
        if(button1.isPressed && button2.isPressed && button3.isPressed && button4.isPressed)
        {
            _isOpen = true;
            GetComponent<Animator>().SetBool("open", true);
        }
    }
}
