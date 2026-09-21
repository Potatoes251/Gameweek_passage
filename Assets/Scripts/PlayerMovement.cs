using UnityEngine;
using FMODUnity;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float speedDivisor;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float iceDeceleration;
    [SerializeField] private float gravityScale;
    [SerializeField] private Animator animator;

    [Header("Footsteps - FMOD")]
    [SerializeField] private EventReference footstepEvent;
    [SerializeField] private float footstepInterval = 0.5f;

    private float slowedSpeed;
    private float currentSpeed;
    private float footstepTimer;

    private CharacterController characterController;
    private Wind wind = null;
    private bool staminaEmpty = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        moveSpeed *= Time.fixedDeltaTime;
        acceleration *= Time.fixedDeltaTime;
        iceDeceleration *= Time.fixedDeltaTime;

        slowedSpeed = moveSpeed / speedDivisor;
        footstepTimer = footstepInterval;

        GameEvents.instance.OnStaminaEmpty += StaminaEmpty;
        GameEvents.instance.OnStaminaNotEmpty += StaminaNotEmpty;
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0f,
            Input.GetAxisRaw("Vertical")
        );

        direction = Vector3.ClampMagnitude(direction, 1f);

        if (direction != Vector3.zero)
        {
            animator.SetBool("walking", true);

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
        }
        else
        {
            animator.SetBool("walking", false);
        }

        if (!characterController.isGrounded)
        {
            direction.y += Physics.gravity.y *
                           Time.fixedDeltaTime *
                           gravityScale;
        }

        if (direction.x != 0 || direction.z != 0)
        {
            currentSpeed += acceleration;

            if (staminaEmpty)
            {
                currentSpeed = Mathf.Min(currentSpeed, slowedSpeed);
            }
            else
            {
                currentSpeed = Mathf.Min(currentSpeed, moveSpeed);
            }
        }
        else
        {
            if (IsOnIce())
            {
                currentSpeed -= iceDeceleration;

                direction = new Vector3(
                    transform.forward.x,
                    direction.y,
                    transform.forward.z
                );
            }
            else
            {
                currentSpeed -= acceleration;
            }

            currentSpeed = Mathf.Max(currentSpeed, 0f);
        }

        characterController.Move(direction * currentSpeed);

        if (wind != null)
        {
            characterController.Move(
                wind.direction * wind.strength
            );
        }

        HandleFootsteps(direction);
    }

    private void HandleFootsteps(Vector3 direction)
    {
        bool isMoving =
            direction.x != 0f ||
            direction.z != 0f;

        if (!isMoving)
        {
            footstepTimer = footstepInterval;
            return;
        }

        if (!characterController.isGrounded)
        {
            return;
        }

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0f)
        {
            PlayFootstep();
            footstepTimer = footstepInterval;
        }
    }

    private void PlayFootstep()
    {
        if (footstepEvent.IsNull)
            return;

        RuntimeManager.PlayOneShot(
            footstepEvent,
            transform.position
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wind"))
        {
            Wind newWind = other.GetComponent<Wind>();

            if (newWind != null)
            {
                wind = newWind;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (
            other.CompareTag("Wind") &&
            other.GetComponent<Wind>() == wind
        )
        {
            wind = null;
        }
    }

    private bool IsOnIce()
    {
        if (!Physics.Raycast(
            transform.position,
            Vector3.down,
            out RaycastHit hit,
            1.3f))
        {
            return false;
        }

        return hit.collider != null &&
               hit.collider.CompareTag("Ice");
    }

    private void StaminaEmpty()
    {
        staminaEmpty = true;
    }

    private void StaminaNotEmpty()
    {
        staminaEmpty = false;
    }

    private void OnDestroy()
    {
        if (GameEvents.instance != null)
        {
            GameEvents.instance.OnStaminaEmpty -= StaminaEmpty;
            GameEvents.instance.OnStaminaNotEmpty -= StaminaNotEmpty;
        }
    }
}