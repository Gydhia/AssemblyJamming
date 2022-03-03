using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public Animator animator;
    public void TriggerAnim()
    {
        if (animator != null)
        {
            animator.SetBool("trigger", true);
        }
    }
}
