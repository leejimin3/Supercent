using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    [SerializeField] float RandomMoveRange = 10.0f;

    private NavMeshAgent navMeshAgent;
    Vector3 RandPos;
    public bool Isdie;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
                    
        if(RandomPoint(transform.position, out RandPos))
        {
            navMeshAgent.destination = RandPos;
        }
        //SetRandomRange();
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
            {
                if(RandomPoint(transform.position, out RandPos))
                {
                    navMeshAgent.destination = RandPos;
                }
            }
                //SetRandomRange();
        }
    }

    bool RandomPoint(Vector3 center, out Vector3 result)    //히트 포지션이 레이어에 맞는 포지션인지 체크하고 맞다면 반환
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * RandomMoveRange;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, 1)) // 1은 NavLayer, 여기서 1은 Walkable Layer
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
    
    // void SetRandomRange()
    // {
    //     var tns = transform.position;
    //     RandPos = new Vector3(Random.Range(tns.x - RandomMoveRange, tns.x + RandomMoveRange), 0,Random.Range(tns.z - RandomMoveRange,tns.z + RandomMoveRange));
    //     navMeshAgent.destination = RandPos;
    // }

}
