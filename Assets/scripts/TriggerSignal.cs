using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSignal : MonoBehaviour
{
    List<Action> _oneTimeTriggers = new List<Action>();
    List<Action> _triggers = new List<Action>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubscribeOneTime(Action action)
    {
        _oneTimeTriggers.Add(action);
    }

    public void Subscribe(Action action)
    {
        _triggers.Add(action);
    }

    private void OnTriggerEnter(Collider other)
    {
        _oneTimeTriggers.ForEach(t => t.Invoke());
        _oneTimeTriggers.Clear();
        _triggers.ForEach(t => t.Invoke());
    }
}
