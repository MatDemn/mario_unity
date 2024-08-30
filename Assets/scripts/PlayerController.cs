using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerTransformState
{
    SMALL = 0,
    NORMAL = 1,
}

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

    bool _isTransforming = false;

    bool _isCannonBalling = false;

    PlayerTransformState _transformState = PlayerTransformState.SMALL;

    float _invincibleTime = 0f;

    float _hurtInvincibleTime = 0f;

    [SerializeField]
    Material _normalMaterial;

    [SerializeField]
    Material _invincibleMaterial;

    [SerializeField]
    Material _goldMaterial;

    [SerializeField]
    SkinnedMeshRenderer _skinnedMeshRenderer;

    [SerializeField]
    Transform _invincibleScalePivot;

    Coroutine _invincibleCoroutine;

    float _currentWalkSpeed = 3f;

    float _normalWalkSpeed = 3f;

    float _speedWalk = 5f;


    float _currentJumpHeight = 5f;

    float _normalJumpHeight = 5f;

    float _goldJumpHeight = 8f;

    bool _isLadder = false;
    
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
        if (_isTransforming || _isCannonBalling) return;

        bool walking = false;
        if (Input.GetKey(KeyCode.W) && !_isLadder)
        {
            _playerModel.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetKey(KeyCode.S) && !_isLadder)
        {
            _playerModel.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (!Physics.Raycast(transform.position, -transform.right, .4f, _obstacleMask))
            {
                walking = true;
                // Ruch w lewo
                transform.position += -Vector3.right * _currentWalkSpeed * Time.deltaTime;
                _playerModel.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
        if (Input.GetKey(KeyCode.D))
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
            rb.AddForce(Vector3.up * _currentJumpHeight, ForceMode.Impulse);
            _isGrounded = false;
            _playerAnim.SetJump(true);
            StartCoroutine(DisableAnimIfGrounded());
        }

        _isGrounded = Physics.Raycast(transform.position, -transform.up, 0.6f);

        _throwCooldown -= _throwCooldown < 0 ? 0 : Time.deltaTime;

        if (_throwCooldown <= 0 && Input.GetKeyDown(KeyCode.LeftShift))
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
        if (_invincibleTime > 0f || _hurtInvincibleTime > 0f) return;

        if (_transformState == PlayerTransformState.SMALL)
        {
            transform.position = _startPosition;
        }
        else // _transformState == PlayerTransformState.NORMAL
        {
            TransformPlayer(PlayerTransformState.SMALL, true);
        }
    }

    public bool TransformPlayer(PlayerTransformState newState, bool hurtInvincibleEffect)
    {
        if (_isTransforming || newState == _transformState) return false;

        _isTransforming = true;

        StartCoroutine(TransformAnim(newState, hurtInvincibleEffect));
        return true;
    }

    IEnumerator TransformAnim(PlayerTransformState newState, bool hurtInvincibleEffect)
    {
        if (newState == _transformState) yield break;

        Vector3 newScale = Vector3.one + Vector3.up / 2;
        Vector3 oldScale = Vector3.one;

        if(newState == PlayerTransformState.SMALL)
        {
            Vector3 temp = newScale;
            newScale = oldScale;
            oldScale = temp;
            if (hurtInvincibleEffect)
            {
                StartCoroutine(SetInvincible(false, 3f));
            }
        }

        for(int i = 0; i<3; i++)
        {
            yield return new WaitForSeconds(.1f);
            _invincibleScalePivot.localScale = newScale;
            yield return new WaitForSeconds(.1f);
            _invincibleScalePivot.localScale = oldScale;
        }
        _invincibleScalePivot.localScale = newScale;
        _transformState = newState;
        _isTransforming = false;
    }

    public void InvincibleStart(float invTime)
    {
        _currentWalkSpeed = _normalWalkSpeed;
        _currentJumpHeight = _normalJumpHeight;
        if(_invincibleCoroutine != null)
        {
            StopCoroutine(_invincibleCoroutine);
            _invincibleCoroutine = null;
            List<Material> normalMaterials = new List<Material>() { _normalMaterial };
            _invincibleTime = 0f;
            _skinnedMeshRenderer.SetMaterials(normalMaterials);
        }
        _invincibleCoroutine = StartCoroutine(SetInvincible(true, invTime));
    }

    IEnumerator SetInvincible(bool isStarInvincible, float invTime)
    {
        _invincibleTime = invTime;
        List<Material> normalMaterials = new List<Material>() { _normalMaterial };
        List<Material> invincibleMaterials = new List<Material>() { _invincibleMaterial };
        List<Material> goldMaterials = new List<Material>() { _goldMaterial };
        if(isStarInvincible)
        {
            _skinnedMeshRenderer.SetMaterials(goldMaterials);
            _currentWalkSpeed = _speedWalk;
            _currentJumpHeight = _goldJumpHeight;
        }
        else
        {
            _skinnedMeshRenderer.SetMaterials(invincibleMaterials);
        }
        while (_invincibleTime > 0f)
        {
            yield return null;
            _invincibleTime -= Time.deltaTime;
        }
        _invincibleTime = 0f;
        _skinnedMeshRenderer.SetMaterials(normalMaterials);
        _currentWalkSpeed = _normalWalkSpeed;
        _currentJumpHeight = _normalJumpHeight;
    }

    public void CannonFire(Vector3 direction)
    {
        _isCannonBalling = true;
        _playerModel.gameObject.SetActive(false);
        StartCoroutine(CoroutineUtils.ExecuteAfter(1f, () =>
        {
            _isCannonBalling = false;
            _playerModel.gameObject.SetActive(true);
            rb.AddForce(direction);
        }));
        
    }

    public void StartLadder()
    {
        rb.useGravity = false;
        _isLadder = true;
        _isGrounded = true;
        _playerModel.rotation = Quaternion.Euler(0, 0, 0);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void MoveLadder(Vector3 direction)
    {
        transform.position += direction * _currentWalkSpeed * Time.deltaTime;
    }

    public void StopLadder()
    {
        rb.useGravity = true;
        _isLadder = false;
    }
}
