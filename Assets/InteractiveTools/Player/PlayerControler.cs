using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    #region Events
   // public event Action<InteractiveObject> NewInteractiveObjectEvent;
    public event Action<string> InteractionMessageEvent;
    public event Action<float> InteractionChargingEvent;
    #endregion

    #region Get

    public Camera Camera => _camera;
    public PlayerHands PlayerHands => playerHands;
    public PlayerItemEQ PlayerItemEQ => playerItemEQ;
    #endregion


    [Header("Player Components")]
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] PlayerItemEQ playerItemEQ;
    [SerializeField] PlayerHands playerHands;
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] Camera _camera;
    [Header("Camera stats")]
    [SerializeField] float mouseSens = 5f;
    [Header("Movement stats")]
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float walkAcceleration = 30f;
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float sprintAcceleration = 50f;
    [SerializeField] float jumpStrenght = 100f;
    [SerializeField] float jumpCd = 0.3f;
    [Header("Move Input")]
    [SerializeField] float fullMoveDeley = 0.6f;
    [Header("Physics stats")]
    [SerializeField] Transform groundPoint;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float maxGroundRange = 0.1f;
    [SerializeField] float dragOnGround = 5f;
    [SerializeField] float dragOffGround = 1f;
    
    Vector3 moveDirection = Vector3.zero;
    float xRotation = 0f;
    float yRotation = 0f;
    float targetSpeed = 1f;
    float acceleration = 0f;
    float lastTimeJump = 0f;
    bool onGround = false;
    float moveDeley = 0;

    KeyCode[] numbKeyCodes = new KeyCode[] {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };

    KeyCode[] itemUseKeys = new KeyCode[] { KeyCode.E, KeyCode.Mouse0, KeyCode.R, KeyCode.Mouse1 };

    #region Init/start/enable/disable
    void Start()
    {
        if (TryGetComponent(out Rigidbody rb))
        {
            _rigidbody = rb;
        }

        Cursor.lockState = CursorLockMode.Locked;
        _camera = Camera.main;

        GameMenager.Instance.PauseGameEvent += GameIsPause;

        if (!groundPoint)
        {
            Debug.LogError("ground Point is not attached");
            groundPoint = transform;
        }
    }

    #endregion
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            GameMenager.Instance.PauseGame();
       

        if (GameMenager.Instance.GameIsPause)
            return;


        xRotation = Input.GetAxis("Mouse X") * mouseSens;

        yRotation -= Input.GetAxis("Mouse Y") * mouseSens;
        yRotation = Mathf.Clamp(yRotation, -90, 90);

        Rotate();

        if (Input.anyKey)
        {
            for (int i = 0; i < numbKeyCodes.Length; ++i)
            {
                if (Input.GetKeyDown(numbKeyCodes[i]))
                {
                    GetItemFromSlot(i);
                    break;
                }
            }

            for (int i = 0; i < itemUseKeys.Length; ++i)
            {
                if (Input.GetKeyDown(itemUseKeys[i]))
                {
                    playerHands.UseItem(itemUseKeys[i]);
                    break;
                }
            }

        }

        if (Input.GetKey(KeyCode.G))
        {
            playerHands.DropItem();
        }

    }

    void FixedUpdate()
    {
        if (GameMenager.Instance.GameIsPause)
            return;

        // player is on ground
        if (Physics.CheckSphere(groundPoint.position, maxGroundRange, groundMask))
        {
            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.z = Input.GetAxis("Vertical");

            if (!Mathf.Approximately(moveDirection.x, 0f) || !Mathf.Approximately(moveDirection.z, 0f))
                moveDeley = Mathf.Clamp(moveDeley + fullMoveDeley * Time.deltaTime, 0, 1);
            else
                moveDeley = 0;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                targetSpeed = sprintSpeed;
                acceleration = sprintAcceleration;
            }
            else
            {
                targetSpeed = walkSpeed;
                acceleration = walkAcceleration;
            }

            if (Input.GetButton("Jump") && Time.fixedTime - lastTimeJump > jumpCd)
                Jump();

            MoveOnGround();
        }
        else
        {
            MoveOffGround();
        }

        if (Input.GetKeyDown(KeyCode.F))
            StartInteract();
    

        if (Input.GetKeyUp(KeyCode.F))
            BrakeInteract();

        playerHands.UpdateHands();
    }

    void StartInteract()
    {
        playerInteraction.StartInteractWithObject();
    }

    void BrakeInteract() 
    {
        playerInteraction.EndInteractWithObject();
    }

    public void UpdateInteract(float interactProgres) 
    {
        InteractionChargingEvent?.Invoke(interactProgres);
    }

    public void NewInteractoveObject(InteractiveObject interactiveObject) 
    {
        if (interactiveObject)
            InteractionMessageEvent?.Invoke(interactiveObject.GetInteractInformation());
        else
            InteractionMessageEvent?.Invoke(null);
    }


    void MoveOnGround()
    {
        

        if (!onGround) 
        {
            onGround = true;
            _rigidbody.drag = dragOnGround;
        }
        

        Vector3 addingForce = transform.right * moveDirection.x;
        addingForce += transform.forward * moveDirection.z;
        addingForce =  acceleration * moveDeley * addingForce.normalized;

        Vector3 newVelocity = _rigidbody.velocity + addingForce * Time.fixedDeltaTime / _rigidbody.mass;
        float newRbVel = new Vector2(newVelocity.x, newVelocity.z).magnitude;

        if (newRbVel > targetSpeed)
        {
            Vector2 flatVel = new Vector2(newVelocity.x, newVelocity.z).normalized * targetSpeed;
            _rigidbody.velocity = new Vector3(flatVel.x, _rigidbody.velocity.y, flatVel.y);
        }
        else
        {
            _rigidbody.AddForce(addingForce);
        }
    }

    void MoveOffGround()
    {

        if (onGround)
        {
            onGround = false;
            _rigidbody.drag = dragOffGround;
        }
    }

    void Jump()
    {
        _rigidbody.AddForce(transform.up * jumpStrenght, ForceMode.Force);
        lastTimeJump = Time.fixedTime;
    }


    void Rotate()
    {
        transform.Rotate(0, xRotation, 0);
        _camera.transform.localRotation = Quaternion.Euler(yRotation, 0, 0);
    }

    void GetItemFromSlot(int slot) 
    {
        playerHands.SwapItemFromEQ(playerItemEQ, slot);
    }

    void GameIsPause(bool pasue) 
    {
        if (pasue)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}

