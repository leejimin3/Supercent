using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    public CapsuleCollider MainBox; 
    public GameObject ThisRig;
    public Animator Anim;

    Collider[] col;
    Rigidbody[] rig;

    void Start()
    {
        GetRagdollBits();
        RagdollOff();
    }

    void GetRagdollBits()
    {
        col = ThisRig.GetComponentsInChildren<Collider>();
        rig = ThisRig.GetComponentsInChildren<Rigidbody>();
    }

    void RagdollOn()
    {
        foreach(Collider c in col)
        {
            c.enabled = true;
        }

        foreach(Rigidbody r in rig)
        {
            r.isKinematic = false;
        }

        Anim.enabled = false;
        MainBox.enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;

    }

    void RagdollOff()
    {
        Anim.enabled = true;


        foreach(Collider c in col)
        {
            c.enabled = false;
        }

        foreach(Rigidbody r in rig)
        {
            r.isKinematic = true;
        }

        
        MainBox.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;

    }
}
