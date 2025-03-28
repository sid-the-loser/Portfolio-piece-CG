using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    
    [SerializeField] float speed = 12f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 3f;
    
    [SerializeField] Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;
    
    [SerializeField] float mouseSensitivity = 100f;
    
    [SerializeField] Transform headGameObject;
    
    float _xRotation = 0f;
    
    bool _isGrounded;
    Vector3 _velocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        _xRotation -= mouseY;
        
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        
        headGameObject.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        
        transform.Rotate(Vector3.up * mouseX);
        
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        
        var move = transform.right * x + transform.forward * z;
        
        controller.Move(move * (speed * Time.deltaTime));

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        _velocity.y += gravity * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);
    }
}
