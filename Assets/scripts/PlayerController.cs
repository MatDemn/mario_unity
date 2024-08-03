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

    [SerializeField]
    bool _isGrounded = true;

    float _throwCooldown = 0f;

    Vector3 _startPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
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

        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 6f, ForceMode.Impulse);
            _isGrounded = false;
            _playerAnim.SetJump(true);
            StartCoroutine(DisableAnimIfGrounded());
        }

        _isGrounded = Physics.Raycast(transform.position, -transform.up, 0.6f);

        _throwCooldown -= _throwCooldown < 0 ? 0 : Time.deltaTime; 

        if(_throwCooldown <= 0 && Input.GetKeyDown(KeyCode.LeftShift))
        {
            _throwCooldown = 3f;
            _playerAnim.TriggerThrow();
        }
    }

    IEnumerator DisableAnimIfGrounded()
    {
        while (true)
        {
            yield return new WaitForSeconds(.1f);
            if (_isGrounded)
            {
                _playerAnim.SetJump(false);
                yield break;
            }      
        }
    }

    public void Death()
    {
        transform.position = _startPosition;
    }
}
