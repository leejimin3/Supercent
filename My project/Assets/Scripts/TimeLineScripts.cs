using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineScripts : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] GameObject _player;
    [SerializeField] ParticleSystem particle;
    
    void Start() 
    {
        float t = particle.startLifetime;
    }
    public void MovCam()
    {
        _camera.transform.position += _player.transform.position;
    }

    void Update() 
    {   
        _camera.transform.RotateAround(_player.transform.position, Vector3.up, 20 * Time.deltaTime);
    }
}
