using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    Transform _modelTransform;

    [SerializeField]
    Transform _endTransform;
    int _direction = 1;
    Vector3 _startPos;

    [SerializeField]
    float _stopTime = 1f;

    [SerializeField]
    float _speed = 1f;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        _startPos = _modelTransform.position;
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Move()
    {
        float progress = 0;
        while(true)
        {
            progress += Time.deltaTime * _direction * _speed;
            rb.MovePosition(Vector3.Lerp(_startPos, _endTransform.position, progress));
            if ((progress >= 1 && _direction == 1) || (progress <= 0 && _direction == -1))
            {
                _direction *= -1;
                if(_stopTime > 0)
                {
                    yield return new WaitForSeconds(_stopTime);
                }
            }
            yield return null;
        }
    }
}
