using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    [SerializeField]
    Transform _rotatingObject;

    AudioSource _audioSource;

    Collider _collider;
    Renderer _renderer;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RotateCoin());
        _audioSource = GetComponent<AudioSource>();
        _collider = GetComponent<Collider>();
        _renderer = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RotateCoin()
    {
        while(true)
        {
            _rotatingObject.Rotate(Vector3.up * 200f * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _audioSource.Play();
        _collider.enabled = false;
        _renderer.enabled = false;
        Destroy(gameObject,1f);
    }
}
