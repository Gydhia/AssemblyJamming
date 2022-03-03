using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EndGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.LoadEndGame();
    }
}
