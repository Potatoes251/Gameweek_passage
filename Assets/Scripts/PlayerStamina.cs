using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class PlayerStamina : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float maxStamina;
    [SerializeField] private float minStamina;
    [SerializeField] private float staminaReduction;
    [SerializeField] private float staminaLostAdditionAtNight;
    [SerializeField] private float staminaAdditionDefault;
    [SerializeField] private float staminaLostAdditionInSnow;
    [SerializeField] private float staminaAdditionInWarmth;
    [SerializeField] private float staminaLostReductionInWarmth;
    [SerializeField] private float movingThreshold = 0.1f;
    [SerializeField] private float staminaGainWaitingTime;
    [SerializeField] private Animator animator;

    [Header("FMOD")]
    [SerializeField] private EventReference noStaminaSound;

    private float stamina;
    private float lostStamina = 0f;

    private bool hasNoStamina = false;
    private bool isNight = false;

    private float staminaGainTimer;
    private int snowCount = 0;

    private CharacterController characterController;

    enum PlayerState
    {
        Default,
        Cold,
        Warm
    }

    private PlayerState playerState = PlayerState.Default;

    enum MovementState
    {
        Resting,
        Moving,
        NotMoving
    }

    private MovementState movementState = MovementState.Resting;

    private void Start()
    {
        stamina = maxStamina;
        characterController = GetComponent<CharacterController>();

        GameEvents.instance.OnEnterSnow += EnterSnow;
        GameEvents.instance.OnExitSnow += ExitSnow;
        GameEvents.instance.OnEnterWarmth += EnterWarmth;
        GameEvents.instance.OnExitWarmth += ExitWarmth;
        GameEvents.instance.OnNightBegin += NightBegin;
        GameEvents.instance.OnNightEnd += NightEnd;
    }

    private void Update()
    {
        CheckPlayerMovement();
        UpdateStamina();
        UpdateStaminaState();
        CheckGameOver();
    }

    private void UpdateStamina()
    {
        switch (playerState)
        {
            case PlayerState.Default:

                if (movementState == MovementState.Resting && stamina < maxStamina)
                {
                    animator.SetBool("resting", true);
                    stamina += staminaAdditionDefault * Time.deltaTime;
                }

                if (isNight)
                {
                    lostStamina += staminaLostAdditionAtNight * Time.deltaTime;
                }

                break;

            case PlayerState.Cold:

                lostStamina += staminaLostAdditionInSnow * Time.deltaTime;

                if (isNight)
                {
                    lostStamina += staminaLostAdditionAtNight * Time.deltaTime;
                }

                break;

            case PlayerState.Warm:

                animator.SetBool("resting", true);
                stamina += staminaAdditionInWarmth * Time.deltaTime;
                lostStamina -= staminaLostReductionInWarmth * Time.deltaTime;

                break;
        }

        if (movementState == MovementState.Moving)
        {
            animator.SetBool("resting", false);
            stamina -= staminaReduction * Time.deltaTime;
        }

        lostStamina = Mathf.Max(lostStamina, 0f);

        stamina = Mathf.Clamp(
            stamina,
            minStamina,
            maxStamina - lostStamina
        );

        if (stamina == maxStamina - lostStamina)
        {
            animator.SetBool("resting", false);
        }
    }

    private void UpdateStaminaState()
    {
        if (stamina <= minStamina && !hasNoStamina)
        {
            hasNoStamina = true;

            GameEvents.instance.StaminaEmpty();

            if (!noStaminaSound.IsNull)
            {
                RuntimeManager.PlayOneShot(
                    noStaminaSound,
                    transform.position
                );
            }
        }
        else if (hasNoStamina && stamina > minStamina)
        {
            hasNoStamina = false;
            GameEvents.instance.StaminaNotEmpty();
        }
    }

    private void CheckGameOver()
    {
        if (lostStamina >= maxStamina)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private bool IsMoving()
    {
        if (characterController == null)
            return false;

        return Mathf.Abs(characterController.velocity.x) > movingThreshold ||
               Mathf.Abs(characterController.velocity.z) > movingThreshold;
    }

    private void CheckPlayerMovement()
    {
        switch (movementState)
        {
            case MovementState.Resting:

                if (IsMoving())
                {
                    movementState = MovementState.Moving;
                    GameEvents.instance.PlayerStartMoving();
                }

                break;

            case MovementState.Moving:

                if (!IsMoving())
                {
                    movementState = MovementState.NotMoving;
                    staminaGainTimer = 0f;
                    GameEvents.instance.PlayerStopMoving();
                }

                break;

            case MovementState.NotMoving:

                staminaGainTimer += Time.deltaTime;

                if (staminaGainTimer >= staminaGainWaitingTime && !IsMoving())
                {
                    movementState = MovementState.Resting;
                }
                else if (IsMoving())
                {
                    movementState = MovementState.Moving;
                    GameEvents.instance.PlayerStartMoving();
                }

                break;
        }
    }

    private void EnterSnow()
    {
        snowCount++;

        if (playerState != PlayerState.Warm &&
            playerState != PlayerState.Cold)
        {
            playerState = PlayerState.Cold;
        }
    }

    private void ExitSnow()
    {
        snowCount--;

        if (playerState == PlayerState.Cold && snowCount <= 0)
        {
            snowCount = 0;
            playerState = PlayerState.Default;
        }
    }

    private void EnterWarmth()
    {
        if (playerState != PlayerState.Warm)
        {
            playerState = PlayerState.Warm;
        }
    }

    private void ExitWarmth()
    {
        if (playerState == PlayerState.Warm)
        {
            playerState = PlayerState.Default;
        }
    }

    private void NightBegin()
    {
        isNight = true;
    }

    private void NightEnd()
    {
        isNight = false;
    }

    private void OnDestroy()
    {
        if (GameEvents.instance == null)
            return;

        GameEvents.instance.OnEnterSnow -= EnterSnow;
        GameEvents.instance.OnExitSnow -= ExitSnow;
        GameEvents.instance.OnEnterWarmth -= EnterWarmth;
        GameEvents.instance.OnExitWarmth -= ExitWarmth;
        GameEvents.instance.OnNightBegin -= NightBegin;
        GameEvents.instance.OnNightEnd -= NightEnd;
    }

    public float GetStamina()
    {
        return stamina;
    }

    public float GetLostStamina()
    {
        return lostStamina;
    }

    public float GetMaxStamina()
    {
        return maxStamina;
    }

    public float GetAvailableStamina()
    {
        return maxStamina - lostStamina;
    }

    public bool HasNoStamina()
    {
        return hasNoStamina;
    }
}