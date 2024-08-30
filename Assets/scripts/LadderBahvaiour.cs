using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderBahvaiour : MonoBehaviour
{
    PlayerController _playerOnLadder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            _playerOnLadder = other.GetComponentInChildren<PlayerController>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == _playerOnLadder.gameObject)
        {
            float moveDir = 0f;
            moveDir += Input.GetKey(KeyCode.W) ? 1 : 0;
            moveDir += Input.GetKey(KeyCode.S) ? -1 : 0;
            Vector3 dir = transform.up * moveDir;
            if(moveDir != 0f)
            {
                _playerOnLadder.StartLadder();
                _playerOnLadder.MoveLadder(dir);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            _playerOnLadder.StopLadder();
            _playerOnLadder = null;
        }
        
    }
}
