using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController _player;
    private float _moveSpeed = 7f;
    private Vector2 _moveInput;
    private Vector3 _moveAxis;
    private Camera _camera;

    private Vector3 _camForward, _camRight, _moveDir;

    [SerializeField] private float _gravity;
    [SerializeField] private float _fallVelocity;
    [SerializeField] private float _jumpForce;

    private Animator _animator;

    [SerializeField] private float _bounceForce = 20f;
    private bool _onBounceSurface = false;

    // 🔊 AUDIO DE PASOS
    private AudioSource _stepsAudio;

    void Start()
    {
        _player = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _camera = Camera.main;

        _gravity = 60f;
        _jumpForce = 15f;

        // Cargar el AudioSource
        _stepsAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        float h = 0, v = 0;
        if (keyboard.aKey.isPressed) h = -1;
        if (keyboard.dKey.isPressed) h = 1;
        if (keyboard.wKey.isPressed) v = 1;
        if (keyboard.sKey.isPressed) v = -1;

        // Actualizamos el movimiento primero
        _moveAxis = new Vector3(h, 0, v).normalized * _moveSpeed;

        // Animator
        _animator.SetFloat("PosX", _moveAxis.x);
        _animator.SetFloat("PosZ", _moveAxis.z);

        // Movimiento relativo a la cámara
        CameraDirection();
        _moveDir = _moveAxis.x * _camRight + _moveAxis.z * _camForward;

        // Rotación del personaje
        if (_moveDir.magnitude > 0.1f)
            transform.LookAt(transform.position + _moveDir);

        // Gravedad y salto
        SetGravity();
        SetJump();

        // Movimiento final
        _player.Move(_moveDir * Time.deltaTime);

        // 🔊 CONTROLAR SONIDO DE PASOS
        HandleFootsteps();
    }

    // ================================================
    //              SONIDO DE PASOS
    // ================================================
    private void HandleFootsteps()
    {
        // El personaje se mueve horizontalmente
        bool isMoving = new Vector3(_moveAxis.x, 0, _moveAxis.z).magnitude > 0.1f;

        // Está tocando el piso
        bool grounded = _player.isGrounded;

        if (isMoving && grounded)
        {
            if (!_stepsAudio.isPlaying)
                _stepsAudio.Play();   // Suena
        }
        else
        {
            if (_stepsAudio.isPlaying)
                _stepsAudio.Stop();    // Se detiene
        }
    }

    private void CameraDirection()
    {
        _camForward = _camera.transform.forward.normalized;
        _camRight = _camera.transform.right.normalized;
        _camForward.y = 0;
        _camRight.y = 0;
    }

    private void SetGravity()
    {
        if (_player.isGrounded)
        {
            if (_onBounceSurface)
            {
                _fallVelocity = _bounceForce;
                _moveDir.y = _fallVelocity;
                _onBounceSurface = false;
            }
            else
            {
                if (_fallVelocity < 0)
                    _fallVelocity = -2f;

                _animator.SetFloat("Jump", 0);
            }
        }
        else
        {
            _fallVelocity -= _gravity * Time.deltaTime;
            _animator.SetFloat("Jump", 1);
        }

        _moveDir.y = _fallVelocity;
    }

    private void SetJump()
    {
        var keyboard = Keyboard.current;
        if (_player.isGrounded && keyboard != null && keyboard.spaceKey.wasPressedThisFrame)
        {
            _fallVelocity = _jumpForce;
            _moveDir.y = _fallVelocity;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Bounce"))
        {
            _onBounceSurface = true;
        }
    }
}
