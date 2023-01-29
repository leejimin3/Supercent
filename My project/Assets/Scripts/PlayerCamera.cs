using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    
    void Awake() //핸드폰의 크기에 따라 9:16비율로 설정
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16);
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;
    }

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
