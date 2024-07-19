using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetJump(bool state)
    {
        _animator.SetBool("Jumping", state);
    }

    public void SetWalking(bool state)
    {
        _animator.SetBool("Walking", state);
    }

    public void TriggerThrow()
    {
        _animator.SetTrigger("Throwing");
    }
}
