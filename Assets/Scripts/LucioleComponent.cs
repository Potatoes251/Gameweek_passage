using UnityEngine;

public class LucioleComponent : MonoBehaviour
{

    private ParticleSystem particleSystem;
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();

        GameEvents.instance.OnNightBegin += Activate;
        GameEvents.instance.OnNightEnd += Disactivate;
    }

    void Activate()
    {
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
    void Disactivate()
    {
        particleSystem.Play();
    }
}
