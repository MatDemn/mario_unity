using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonBehaviour : MonoBehaviour
{
    float _delay = 0f;

    [SerializeField]
    Transform _fireDirection;

    [SerializeField]
    Transform _model;

    [SerializeField]
    ParticleSystem _particleSystem;

    [SerializeField]
    float _force = 50f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        _delay = Mathf.Clamp(_delay - Time.deltaTime, 0, Mathf.Infinity);
        if(_delay == 0f && _particleSystem.isPlaying)
        {
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_delay > 0f) return;
        if(other.gameObject.CompareTag("Player"))
        {
            other.GetComponentInChildren<PlayerController>().CannonFire(_fireDirection.forward * _force);
            StartCoroutine(CoroutineUtils.ExecuteAfter(1f, () =>
            {
                _model.localScale = Vector3.one;
                _particleSystem.Play();
            }));
            _model.localScale = Vector3.one * 1.2f;
            _delay = 5f;
        }
    }
}
