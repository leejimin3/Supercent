using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObj : MonoBehaviour
{
        Transform target;
        Vector3 relativeVector;

        // private Transform spine;
        // private Animator anim;

        List<GameObject> OverLapActor = new List<GameObject>();

        void Start()
        {
            //spine = anim.GetBoneTransform(HumanBodyBones.Spine);

        }

        public void DetectActor(Collider other, Vector3 pos)
        {
            OverLapActor.Add(other.gameObject);

            Debug.Log(OverLapActor.Count);

            if(OverLapActor == null)
                return;

            GameObject closest = OverLapActor[0];
            
            foreach(var CurrentActor in OverLapActor)
            {
                if(CurrentActor != null)
                {
                    if(Vector3.Distance(pos, closest.GetComponent<Transform>().position) < Vector3.Distance(pos, CurrentActor.GetComponent<Transform>().position))
                    {
                        closest = CurrentActor;
                    }
                }
            }

            target = closest.GetComponent<Transform>();
            Debug.Log(target);
        }




        public void DetectActor(Collider other)
        {
            if(OverLapActor != null)
            {
                for(int i = 0; i < OverLapActor.Count; i++)
                {
                    if(OverLapActor[i] == null)
                        continue;
                        
                    if(other.gameObject == OverLapActor[i].gameObject)
                    {
                        OverLapActor[i] = null;
                    }
                }
            }

        }

}
