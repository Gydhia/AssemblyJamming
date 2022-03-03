using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PickableItem : MonoBehaviour
{
    public GameObject mesh;
    public Item item;
    private SphereCollider sc;
    public SphereCollider Sc => sc;
    // Start is called before the first frame update
    void Awake()
    {
        sc = GetComponent<SphereCollider>();
        mesh = GameObject.Instantiate(mesh, transform);
    }
}
