using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonAnimator : MonoBehaviourPun
{
    public Animator animator;

    [PunRPC]
    public void SetTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }
    [PunRPC]
    public void SetFloat(string key, float value)
    {
        animator.SetFloat(key, value);
    }

}
