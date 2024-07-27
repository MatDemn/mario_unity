using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeEnemy : MonoBehaviour
{
    float _waitTime = 5f;

    float _showTime = 3f;

    float _speed = 1f;

    [SerializeField]
    Transform _enemyModel;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EnemyCoroutine()
    {
        float progress = 0f;
        while(true)
        {
            yield return new WaitForSeconds(_waitTime);
            while(progress < 1f)
            {
                progress += Time.deltaTime * _speed;
                _enemyModel.localPosition = Vector3.Lerp(Vector3.zero, Vector3.up, progress);
                yield return null;
            }
            yield return new WaitForSeconds(_showTime);
            while(progress > 0f)
            {
                progress -= Time.deltaTime * _speed;
                _enemyModel.localPosition = Vector3.Lerp(Vector3.zero, Vector3.up, progress);
                yield return null;
            }
        }
    }
}
