using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines.Interpolators;

public class DayNightCircle : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField, Range(0f, 360f)]
    private float   dayRotation = 0;
    [SerializeField, Range(0f, 360f)]
    private float   nightRotation = 0;
    [SerializeField]
    private float   dayDuration = 0;
    [SerializeField]
    private float   nightDuration = 0;

    private float       cycleElapse = 0;
    private bool        isDay = true;
    private Vector3     rotation;

    void Start()
    {
        transform.rotation = Quaternion.Euler(dayRotation, transform.eulerAngles.y, transform.eulerAngles.z);

        Vector3 rotation = transform.localEulerAngles;
    }

    void Update()
    {
        cycleElapse += Time.deltaTime;

        if (isDay)
        {
            rotation.x = Mathf.Lerp(dayRotation, nightRotation, cycleElapse / dayDuration);

            if (cycleElapse >= dayDuration)
            {
                isDay = false;
                cycleElapse = 0;
                GameEvents.instance.NightBegin();
            }
        }
        else
        {
            rotation.x = Mathf.Lerp(nightRotation, dayRotation + 360f, cycleElapse / nightDuration);

            if (cycleElapse >= nightDuration)
            {
                isDay = true;
                cycleElapse = 0;
                GameEvents.instance.NightEnd();
            }
        }

        transform.localEulerAngles = rotation;
    }
}