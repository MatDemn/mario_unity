using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    Vector3 _patrolStart;

    [SerializeField]
    Transform _patrolEnd;

    [SerializeField]
    Transform _modelTransform;

    [SerializeField]
    Transform _diePivot;

    int _direction = 1;

    Animator _animator;

    [SerializeField]
    [Range(0.01f, 5f)]
    float _speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        _patrolStart = _modelTransform.position;
        _animator = GetComponentInChildren<Animator>();
        StartCoroutine(Patrol());
        GetComponentInChildren<TriggerSignal>().SubscribeOneTime(Die);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Patrol()
    {
        float progress = 0;
        _animator.SetBool("Walking", true);
        while (true)
        {
            _modelTransform.position = Vector3.Lerp(_patrolStart, _patrolEnd.position, progress);

            progress += Time.deltaTime * _direction * _speed ;

            _modelTransform.rotation = Quaternion.Euler(0, _direction == 1 ? 0 : 180, 0);

            if ((progress >= 1 && _direction == 1) || (progress <= 0 && _direction == -1))
            {
                _direction *= -1;
            }

            yield return null;
        }
    }

    IEnumerator ScaleDown()
    {
        float progress = _diePivot.localScale.y;
        while(true)
        {
            float delta = -Time.deltaTime;
            _diePivot.localScale += Vector3.up * delta;
            progress += delta;
            if (progress <= 0)
            {
                Destroy(gameObject);
                yield break;
            }
            yield return null;
            
        }
    }

    public void Die()
    {
        StopAllCoroutines();
        var colliders = GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
        _animator.SetBool("Walking", false);
        StartCoroutine(ScaleDown());
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogError($"{collision.gameObject.tag}, {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Fireball"))
        {
            Die();
        }
    }
}
