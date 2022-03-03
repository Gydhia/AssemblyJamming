using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlacementHelper : MonoBehaviour
{
    // Update is called once per frame
     void Update()
    {
        Debug.DrawRay(transform.position, new Vector3(0f, 0f, transform.position.y + 5f));
        Debug.DrawRay(transform.position, new Vector3(0f, transform.position.z + 5f, 0f));
    }
}
