using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    static readonly int ANIM_Hit = Animator.StringToHash("Hit");

    [SerializeField] float MaxHealth = 15.0f;
    [SerializeField] float CurrentHealth = 15.0f;
    [SerializeField] float KnockBackPower = 500.0f;

    [SerializeField] Collider Thiscol; 
    [SerializeField] Rigidbody ThisRig;
    [SerializeField] Animator Anim;
    [SerializeField] Slider hpslider;
    [SerializeField] GameObject canvas;

    [SerializeField] Rigidbody knockback = null;

    Collider[] col = null;
    Rigidbody[] rig = null;

    void Awake()
    {
        CurrentHealth = MaxHealth;
        GetRagdollBits();
        SetRagdoll(false);
    }

    void Update()
    {
        canvas.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        hpslider.value = Mathf.Lerp(hpslider.value, CurrentHealth / MaxHealth, Time.deltaTime * 5f);
    }

    void GetRagdollBits()
    {
        col = transform.Find("Armature").GetComponentsInChildren<Collider>();
        rig = transform.Find("Armature").GetComponentsInChildren<Rigidbody>();
    }

    void SetRagdoll(bool act)
    {
        Anim.enabled = !act;
        ThisRig.isKinematic = act;
        Thiscol.isTrigger = act;


        for(int i = 0, cnt = rig.Length; i < cnt; i++)
        {
            var rigid = rig[i];
            if(null == rigid)
                continue;

            rigid.isKinematic = !act;
        }

        for(int i = 0, cnt = col.Length; i < cnt; i++)
        {
            var collider = col[i];
            if(null == collider)
                continue;

            collider.isTrigger = !act;
        }
    }

    public bool TakeDamage(float damage, GameObject obj)
    {
        canvas.SetActive(true);
        CurrentHealth -= damage;
        Anim.SetTrigger(ANIM_Hit);
        //애니
        if (CurrentHealth <= 0f)
        {
            SetRagdoll(true);
            GameManager.instance.EnemyDie();

            var dir = transform.position - obj.transform.position;
            Debug.Log(dir);
            this.tag = "Untagged";
            knockback.AddForce(dir * KnockBackPower + Vector3.up * 3f * KnockBackPower);
            Invoke("CanvasClose", 2.0f);
            Invoke("SinkEnemy", 3.0f);
            return true;
        }
        Invoke("CanvasClose", 2.0f);

        return false;
    }

    void CanvasClose()
    {
        canvas.SetActive(false);
    }

    void SinkEnemy()
    {
        for (int i = 0, cnt = rig.Length; i < cnt; i++)
        {
            var rigid = rig[i];
            if (null == rigid)
                continue;

            rigid.isKinematic = false;
        }

        for (int i = 0, cnt = col.Length; i < cnt; i++)
        {
            var collider = col[i];
            if (null == collider)
                continue;

            collider.isTrigger = true;
        }
        Destroy(this.gameObject, 2.0f);
    }

    // [SerializeField] float MaxHealth = 15.0f;
    // [SerializeField] float CurrentHealth = 15.0f;

    // public CapsuleCollider Thiscol; 
    // public GameObject ThisRig;
    // public Animator Anim;

    // Collider[] col;
    // Rigidbody[] rig;

    // void Start()
    // {
    //     CurrentHealth = MaxHealth;
    //     GetRagdollBits();
    //     RagdollOff();
    // }

    // void Update()
    // {
        
    // }

    // void GetRagdollBits()
    // {
    //     col = ThisRig.GetComponentsInChildren<Collider>();
    //     rig = ThisRig.GetComponentsInChildren<Rigidbody>();
    // }

    // void RagdollOn()
    // {
    //     foreach(Collider c in col)
    //     {
    //         c.enabled = true;
    //     }

    //     foreach(Rigidbody r in rig)
    //     {
    //         r.isKinematic = false;
    //     }

    //     Anim.enabled = false;
    //     Thiscol.enabled = true;

    // }

    // void RagdollOff()
    // {
    //     Anim.enabled = true;


    //     foreach(Collider c in col)
    //     {
    //         c.isTrigger = true;
    //     }

    //     foreach(Rigidbody r in rig)
    //     {
    //         r.isKinematic = true;
    //     }

        
    //     Thiscol.enabled = true;
    //     GetComponent<Rigidbody>().isKinematic = true;

    // }

    // public bool TakeDamage(float damage, GameObject obj)
    // {
    //     CurrentHealth -= damage;

    //     if(CurrentHealth <= 0f)
    //     {
    //         Vector3 vec = obj.transform.forward;
    //         RagdollOn();
    //         GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Spine).GetComponent<Rigidbody>().AddForce(vec * 10000f);
    //         GameManager.instance.EnemyDie();
    //         return true;
    //     }
    //     return false;    
    // }
}
