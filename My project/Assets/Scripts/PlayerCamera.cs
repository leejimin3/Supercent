using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate() // >> LateUpdate
    {
        transform.position = player.transform.position + offset;
    }

    public void Shake()
    {
        StartCoroutine(CameraShake(0.1f, 0.1f)); //0.1초 동안 0.1의 범위로
    }

    IEnumerator CameraShake(float time, float mag)
    {
        float timer = 0.0f;
        Vector3 pos = transform.position;
        while(timer <= time)
        {
            transform.localPosition = Random.insideUnitSphere * mag + pos;

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = pos;
    }
}
