using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    LayerMask _obstacleMask;

    PlayerAnim _playerAnim;

    [SerializeField]
    Transform _playerModel;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _obstacleMask = LayerMask.GetMask("obstacle");
        _playerAnim = GetComponentInChildren<PlayerAnim>();
    }

    // Update is called once per frame
    void Update()
    {
        bool walking = false;
        if(Input.GetKey(KeyCode.W))
        {
            _playerModel.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            _playerModel.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            if(!Physics.Raycast(transform.position, -transform.right, .4f, _obstacleMask))
            {
                walking = true;
                // Ruch w lewo
                transform.position += -Vector3.right * 3f * Time.deltaTime;
                _playerModel.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
        if(Input.GetKey(KeyCode.D))
        {
            if (!Physics.Raycast(transform.position, transform.right, .4f, _obstacleMask))
            {
                walking = true;
                // Ruch w prawo
                transform.position += Vector3.right * 3f * Time.deltaTime;
                _playerModel.rotation = Quaternion.Euler(0, 90, 0);
            }
                
        }

        _playerAnim.SetWalking(walking);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 6f, ForceMode.Impulse);
            _playerAnim.SetJump(true);
            StartCoroutine(CheckForGround());
        }

        
    }

    IEnumerator CheckForGround()
    {
        while(true)
        {
            yield return new WaitForSeconds(.1f);
            if (Physics.Raycast(transform.position, -transform.up, 1f))
            {
                _playerAnim.SetJump(false);
                yield break;
            }
        }
    }
}
