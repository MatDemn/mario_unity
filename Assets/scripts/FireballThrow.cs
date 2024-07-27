using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballThrow : MonoBehaviour
{
    [SerializeField]
    GameObject _fireballPrefab;

    [SerializeField]
    Transform _fireballTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Throw()
    {
        GameObject go = Instantiate(_fireballPrefab, _fireballTransform.position, _fireballTransform.rotation);
        go.GetComponent<Rigidbody>().AddForce(_fireballTransform.forward*5f, ForceMode.Impulse);
    }
}
