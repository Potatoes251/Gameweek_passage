using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    private void Awake()
    {
        instance = this;
    }

    //player events
    public event Action OnEnterSnow;
    public void EnterSnow()
    {
        if (OnEnterSnow != null)
        {
            OnEnterSnow();
        }
    }

    public event Action OnExitSnow;
    public void ExitSnow()
    {
        if (OnExitSnow != null)
        {
            OnExitSnow();
        }
    }

    public event Action OnEnterWarmth;
    public void EnterWarmth()
    {
        if (OnEnterWarmth != null)
        {
            OnEnterWarmth();
        }
    }

    public event Action OnExitWarmth;
    public void ExitWarmth()
    {
        if (OnExitWarmth != null)
        {
            OnExitWarmth();
        }
    }

    public event Action OnStaminaEmpty;
    public void StaminaEmpty()
    {
        if (OnStaminaEmpty != null)
        {
            OnStaminaEmpty();
        }
    }

    public event Action OnStaminaNotEmpty;
    public void StaminaNotEmpty()
    {
        if (OnStaminaNotEmpty != null)
        {
            OnStaminaNotEmpty();
        }
    }

    public event Action OnPlayerStartMoving;
    public void PlayerStartMoving()
    {
        if (OnPlayerStartMoving != null)
        {
            OnPlayerStartMoving();
        }
    }

    public event Action OnPlayerStopMoving;
    public void PlayerStopMoving()
    {
        if (OnPlayerStopMoving != null)
        {
            OnPlayerStopMoving();
        }
    }

    public event Action OnNightBegin;
    public void NightBegin()
    {
        if (OnNightBegin != null)
        {
            OnNightBegin();
        }
    }
    public event Action OnNightEnd;
    public void NightEnd()
    {
        if (OnNightEnd != null)
        {
            OnNightEnd();
        }
    }
}
