using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBehaviour : MonoBehaviour
{
    [SerializeField]
    Transform _modelTransform;

    float _speed = 5f;

    bool _ready = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ScaleModel()
    {
        _ready = false;
        float progress = 0f;
        while(progress < 1f)
        {
            progress += Time.deltaTime * _speed;
            _modelTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one + Vector3.up * 1.1f, progress);
            yield return null;
        }
        while(progress > 0f)
        {
            progress -= Time.deltaTime * _speed;
            _modelTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one + Vector3.up * 1.1f, progress);
            yield return null;
        }
        _ready = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_ready) return;
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if(rb != null)
        {
            StartCoroutine(ScaleModel());
            rb.AddForce(transform.up * 13f, ForceMode.Impulse);
            
        }
    }
}
