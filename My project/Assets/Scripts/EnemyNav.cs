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

    // Update is called once per frame
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
        RandPos = new Vector3(Random.Range(-14f, 14f), 0,Random.Range(-10f, 18f));
        navMeshAgent.destination = RandPos;
    }

}
