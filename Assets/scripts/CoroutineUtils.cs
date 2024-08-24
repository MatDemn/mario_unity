using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineUtils : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static IEnumerator ExecuteAfter(float delay, Action f)
    {
        yield return new WaitForSeconds(delay);
        f.Invoke();
    }
}
