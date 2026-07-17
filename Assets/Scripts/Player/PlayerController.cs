using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [Tooltip("Karakterin sabit ileri koþu hýzý ")]
    public float forwardSpeed = 8f;

    [Tooltip("Yerçekimi kuvveti ")]
    public float gravity = -20f;

    [Header("Þerit Ayarlarý")]
    [Tooltip("Þeritler arasý mesafe ")]
    public float laneDistance = 3f;

    [Tooltip("Bir þeritten diðerine geçiþ hýzý")]
    public float laneChangeSpeed = 10f;

    [Header("Zýplama Ayarlarý")]
    [Tooltip("Zýplama anýndaki dikey hýz")]
    public float jumpForce = 9f;

    [Header("Eðilme Ayarlarý")]
    [Tooltip("Eðilme durumunun ne kadar süreceði ")]
    public float slideDuration = 0.8f;

    [Tooltip("Eðilme sýrasýndaki CharacterController yüksekliði")]
    public float slideHeight = 1f;

    [Header("Swipe Ayarlarý")]
    public float swipeThreshold = 50f;

    // -1 = sol þerit, 0 = orta þerit, 1 = sað þerit
    private int currentLane = 0;
    private float targetX;

    private CharacterController controller;
    private float verticalVelocity;

    private Vector2 touchStartPos;
    private bool isTouching;

    
    private float standingHeight;
    private Vector3 standingCenter;
    private Vector3 standingScale;
    private bool isSliding;

    
    private float footOffset;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

      
        standingHeight = controller.height;
        standingCenter = controller.center;
        standingScale = transform.localScale;

        footOffset = standingCenter.y - (standingHeight / 2f);
    }

    void Update()
    {
        
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        HandleSwipeInput();
        HandleGravity();
        Move();
    }

   
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TriggerGameOver();
            }
        }
    }

    private void HandleSwipeInput()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 currentPos = Touchscreen.current.primaryTouch.position.ReadValue();

            if (!isTouching)
            {
                touchStartPos = currentPos;
                isTouching = true;
            }
        }
        else if (isTouching)
        {
            isTouching = false;
            Vector2 currentPos = Touchscreen.current != null
                ? Touchscreen.current.primaryTouch.position.ReadValue()
                : touchStartPos;

            Vector2 delta = currentPos - touchStartPos;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                if (Mathf.Abs(delta.x) > swipeThreshold)
                {
                    ChangeLane(delta.x > 0f ? 1 : -1);
                }
            }
            else
            {
                if (Mathf.Abs(delta.y) > swipeThreshold)
                {
                    if (delta.y > 0f) Jump();
                    else Slide();
                }
            }
        }

        
        if (Keyboard.current != null)
        {
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame) ChangeLane(1);
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame) ChangeLane(-1);
            if (Keyboard.current.spaceKey.wasPressedThisFrame) Jump();
            if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame) Slide();
        }
    }

    private void ChangeLane(int direction)
    {
        int newLane = Mathf.Clamp(currentLane + direction, -1, 1);
        currentLane = newLane;
        targetX = currentLane * laneDistance;
    }

    private void Jump()
    {
        
        if (controller.isGrounded && !isSliding)
        {
            verticalVelocity = jumpForce;
        }
    }

    private void Slide()
    {
       
        if (isSliding || !controller.isGrounded) return;

        StartCoroutine(SlideCoroutine());
    }

    private IEnumerator SlideCoroutine()
    {
        isSliding = true;

   
        controller.height = slideHeight;
        controller.center = new Vector3(standingCenter.x, footOffset + (slideHeight / 2f), standingCenter.z);

        
        transform.localScale = new Vector3(standingScale.x, standingScale.y * (slideHeight / standingHeight), standingScale.z);

        yield return new WaitForSeconds(slideDuration);

        
        controller.height = standingHeight;
        controller.center = standingCenter;
        transform.localScale = standingScale;

        isSliding = false;
    }

    private void HandleGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void Move()
    {
        float currentX = transform.position.x;
        float newX = Mathf.Lerp(currentX, targetX, laneChangeSpeed * Time.deltaTime);
        float deltaX = newX - currentX;

        Vector3 move = Vector3.forward * forwardSpeed;
        move.x = deltaX / Time.deltaTime;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }
}