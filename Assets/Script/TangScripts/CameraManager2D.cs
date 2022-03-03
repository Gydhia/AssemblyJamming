using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager2D : MonoBehaviour
{
    public Transform TargetTransform;
    public float Smooth = 1;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.Lerp(
            this.transform.position,
            new Vector3(TargetTransform.position.x, TargetTransform.position.y, this.transform.position.z),
            Smooth);
    }
}
