using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{

    Coroutine _movingCoroutine = null;

    [SerializeField]
    Transform _platformModel;

    float _speed = 7f;

    float _moveLimit = .1f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<TriggerSignal>().Subscribe(RunFalling);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RunFalling()
    {
        if (_movingCoroutine != null) return;

        _movingCoroutine = StartCoroutine(FallingAnim());
    }

    IEnumerator FallingAnim()
    {
        for(int i = 0; i< 3; i++)
        {
            float progress = .5f;
            while(progress < 1f)
            {
                progress += Time.deltaTime * _speed;
                _platformModel.localPosition = Vector3.Lerp(Vector3.left * _moveLimit, Vector3.right * _moveLimit, progress);
                yield return null;
            }
            progress = 1f;
            while(progress > 0f)
            {
                progress -= Time.deltaTime * _speed;
                _platformModel.localPosition = Vector3.Lerp(Vector3.left * _moveLimit, Vector3.right * _moveLimit, progress);
                yield return null;
            }
            progress = 0f;
            while(progress < .5f)
            {
                progress += Time.deltaTime * _speed;
                _platformModel.localPosition = Vector3.Lerp(Vector3.left * _moveLimit, Vector3.right * _moveLimit, progress);
                yield return null;
            }
        }
        _platformModel.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down, ForceMode.Impulse);
        _platformModel.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        _platformModel.gameObject.SetActive(true);
        _movingCoroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            RunFalling();
        }
    }
}
