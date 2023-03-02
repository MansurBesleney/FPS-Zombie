using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Control Settings")]
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private float gravityModifier = 0.95f;
    [SerializeField] private float jumpPower = 0.25f;

    [Header("Mouse Control Settings")]
    [SerializeField] private float mouseSensivity = 1f;
    [SerializeField] private float maxViewAngle = 60f;

    private CharacterController characterController;
    private float currentSpeed = 8f;
    private float horizontalInput;
    private float verticalInput;
    private Transform mainCamera;

    private Vector3 heightMovement;
    private bool isJumping = false;
    
    void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if(Camera.main.GetComponent<CameraController>() == null)
        {
            Camera.main.gameObject.AddComponent<CameraController>();
        }
        mainCamera = GameObject.FindWithTag("CameraPoint").transform;
    }
    void Update()
    {
        KeyboardInput();
    }

    private void FixedUpdate()
    {
        Move();

        Rotate();
    }

    private void Rotate()
    {
        //oyuncunun sağa sola rotasyonunu ayarlıyoruz
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x , transform.eulerAngles.y + MouseInput().x , transform.eulerAngles.z);
        
        if(mainCamera != null)//oyuncunun dikeyde baktığı yönü ayarlıyoruz
        {

            if(mainCamera.eulerAngles.x > maxViewAngle && mainCamera.eulerAngles.x <180f)
            {
                mainCamera.rotation = Quaternion.Euler(maxViewAngle, transform.eulerAngles.y, transform.eulerAngles.z);
            }
            else if(mainCamera.eulerAngles.x > 180f && mainCamera.eulerAngles.x < 360f - maxViewAngle)
            {
                mainCamera.rotation = Quaternion.Euler(360f- maxViewAngle , transform.eulerAngles.y, transform.eulerAngles.z);
            }
            else
            {
                mainCamera.rotation = Quaternion.Euler(mainCamera.rotation.eulerAngles + new Vector3(-MouseInput().y , 0f, 0f));
            }
        }
    }

    private void Move()
    {

        if(isJumping)
        {
            heightMovement.y = jumpPower;
            isJumping = false;
        }

        heightMovement.y -= gravityModifier * Time.deltaTime;
        Vector3 localVerticalVector = transform.forward * verticalInput;
        Vector3 localHorizontalVector = transform.right * horizontalInput;
        Vector3 movementVector = localHorizontalVector + localVerticalVector;

        movementVector.Normalize();
        movementVector *= currentSpeed * Time.deltaTime;
        characterController.Move(movementVector + heightMovement);

        /*
        if(characterController.isGrounded)
        {
            heightMovement.y = 0f;
        }
        */
    }

    private void KeyboardInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            isJumping = true;
        }

        if(Input.GetKey(KeyCode.LeftShift)) 
        {
            currentSpeed = runSpeed;
        }
        else 
        {
            currentSpeed = walkSpeed;
        }
    }

    private Vector2 MouseInput()
    {
        return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensivity;
    }
}
