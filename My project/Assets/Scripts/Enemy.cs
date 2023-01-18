using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float MaxHealth = 15.0f;
    [SerializeField] float CurrentHealth = 15.0f;

    public CapsuleCollider MainBox; 
    public GameObject ThisRig;
    public Animator Anim;

    Collider[] col;
    Rigidbody[] rig;

    void Start()
    {
        CurrentHealth = MaxHealth;
        GetRagdollBits();
        RagdollOff();
    }

    void Update()
    {
        
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
        GetComponent<Rigidbody>().isKinematic = true;

    }

    public void TakeDamage(float damage, GameObject obj)
    {
        CurrentHealth -= damage;

        if(CurrentHealth <= 0f)
        {
            Vector3 vec = obj.transform.forward;
            RagdollOn();
            GetComponent<Rigidbody>().AddForce(vec * 100f);
            
        }
            
    }
}
