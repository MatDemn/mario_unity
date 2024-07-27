using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBehaviour : MonoBehaviour
{
    float _beatSpeed = 5f;
    float _waitTime = 3f;
    float _rotateSpeed = .2f;
    [SerializeField]
    Transform _modelTransform;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PoundingLoop());
        StartCoroutine(RotateHeart());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator PoundingLoop()
    {
        float progress = 0f;
        while (true)
        {
            for (int i = 0; i < 2; i++)
            {
                while (progress < 1f)
                {
                    progress += Time.deltaTime * _beatSpeed;
                    _modelTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, progress);
                    yield return null;
                }
                while (progress > 0f)
                {
                    progress -= Time.deltaTime * _beatSpeed;
                    _modelTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, progress);
                    yield return null;
                }
            }
            yield return new WaitForSeconds(_waitTime);
        }
    }

    IEnumerator RotateHeart()
    {
        float progress =.5f;
        while (true)
        {
            while (progress < 1f)
            {
                progress += Time.deltaTime * _rotateSpeed;
                _modelTransform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.up * -45f, Vector3.up * 45f, progress));
                yield return null;
            }
            while (progress > 0f)
            {
                progress -= Time.deltaTime * _rotateSpeed;
                _modelTransform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.up * -45f, Vector3.up * 45f, progress));
                yield return null;
            }
            while (progress < .5f)
            {
                progress += Time.deltaTime * _rotateSpeed;
                _modelTransform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.up * -45f, Vector3.up * 45f, progress));
                yield return null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
