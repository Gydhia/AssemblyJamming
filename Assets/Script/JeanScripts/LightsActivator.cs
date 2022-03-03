using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsActivator : MonoBehaviour
{
    [SerializeField]
    Light[] lights;

    // Start is called before the first frame update
    void Start()
    {
        lights = GetComponentsInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i =0; i< GameManager.Instance.unlocked.Length ; i++)
        {
            if (GameManager.Instance.unlocked[i] == 1)
            {
                lights[i].color = Color.green;
            }

        }
    }
}
