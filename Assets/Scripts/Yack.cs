using UnityEngine;
using UnityEngine.AI;
using FMODUnity;

public class Yack : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private GameObject player;
    [SerializeField] private float timeToFollow;
    [SerializeField] private Animator animator;

    [Header("Footsteps - FMOD")]
    [SerializeField] private EventReference footstepEvent;
    [SerializeField] private float footstepInterval = 0.5f;

    private NavMeshAgent navAgent;

    private float forgetTime = 1f;
    private float forgetTimer = 0f;
    private float followTimer = 0f;
    private float footstepTimer = 0f;

    private bool isPlayerMoving = false;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();

        if (navAgent == null)
        {
            Debug.LogWarning("Yak is missing the nav agent.");
            return;
        }

        navAgent.SetDestination(player.transform.position);

        footstepTimer = footstepInterval;

        GameEvents.instance.OnPlayerStartMoving += PlayerStartMoving;
        GameEvents.instance.OnPlayerStopMoving += PlayerStopMoving;
    }

    private void Update()
    {
        if (isPlayerMoving && followTimer < timeToFollow)
        {
            animator.SetBool("walking", false);
            followTimer += Time.deltaTime;
        }
        else if (isPlayerMoving)
        {
            animator.SetBool("walking", true);
            navAgent.SetDestination(player.transform.position);
        }
        else if (forgetTimer < forgetTime)
        {
            forgetTimer += Time.deltaTime;
            animator.SetBool("walking", false);
        }
        else
        {
            animator.SetBool("walking", true);
            navAgent.SetDestination(player.transform.position);
        }

        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        if (!navAgent.isOnNavMesh)
            return;

        bool isMoving = navAgent.velocity.sqrMagnitude > 0.01f;

        if (!isMoving)
        {
            footstepTimer = footstepInterval;
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
        Debug.Log("Yack Footstep");

        if (footstepEvent.IsNull)
        {
            Debug.LogWarning("Yack footstep event is not assigned.");
            return;
        }

        RuntimeManager.PlayOneShotAttached(
            footstepEvent,
            gameObject
        );
    }

    private void PlayerStartMoving()
    {
        isPlayerMoving = true;
        followTimer = 0f;
    }

    private void PlayerStopMoving()
    {
        isPlayerMoving = false;
        forgetTimer = 0f;

        if (navAgent != null && navAgent.isOnNavMesh)
        {
            navAgent.SetDestination(player.transform.position);
        }
    }

    private void OnDestroy()
    {
        if (GameEvents.instance != null)
        {
            GameEvents.instance.OnPlayerStartMoving -= PlayerStartMoving;
            GameEvents.instance.OnPlayerStopMoving -= PlayerStopMoving;
        }
    }
}