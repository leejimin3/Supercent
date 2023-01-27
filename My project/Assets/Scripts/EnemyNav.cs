using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    Vector3 RandPos;
    public bool Isdie;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetRandomRange();
    }


    void Update()
    {
        if(Isdie)
        {
            navMeshAgent.destination = transform.position;
        }
        if(!Isdie)
        {
            if(Vector3.Distance(transform.position, RandPos) <= 0.1f)
                SetRandomRange();
        }
    }
    
    void SetRandomRange()
    {
        var tns = transform.position;
        RandPos = new Vector3(Random.Range(-13f, 17f), 0,Random.Range(-9f, 39f));
        navMeshAgent.destination = RandPos;
    }

}
