using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingManager : MonoBehaviour
{
    [System.Serializable]
    private enum ProfileID
    {
        None,
        Cold,
        Warm,
    }
    [System.Serializable]
    private struct Effect
    {
        public ProfileID id;
        public VolumeProfile Profile;
        [Range(0f, 1f)]
        public float Weight;
        [Range(0f, 60f)]
        public float InitiationDuration;
    }

    [Header("Setting")]
    [SerializeField]
    private List<Effect> postProcessEffects;

    private int effectIndex;
    private float tempWeight;
    private float exitDuration = 1f;
    private float enterTimer = 0f;
    private float exitTimer = 99999999f;
    private ProfileID usedProfile = ProfileID.None;

    private Volume volume;

    void Start()
    {
        volume = GetComponent<Volume>();

        GameEvents.instance.OnEnterSnow += EnterSnow;
        GameEvents.instance.OnExitSnow += ExitSnow;
        GameEvents.instance.OnEnterWarmth += EnterWarmth;
        GameEvents.instance.OnExitWarmth += ExitWarmth;
    }


    void Update()
    {
        if (exitDuration > exitTimer)
        {
            exitTimer += Time.deltaTime;
            volume.weight = tempWeight * (1 - exitTimer / exitDuration);
        }

        if (usedProfile == ProfileID.None) { return; }

        else if (exitDuration <= exitTimer && postProcessEffects[effectIndex].InitiationDuration > enterTimer)
        {
            enterTimer += Time.deltaTime;
            volume.weight = postProcessEffects[effectIndex].Weight * (enterTimer / postProcessEffects[effectIndex].InitiationDuration);
        }
    }

    void SetEffect(ProfileID target)
    {
        for (int index = 0; index < postProcessEffects.Count; index++)
        {
            if (postProcessEffects[index].id == target)
            {
                effectIndex = index;
                volume.profile = postProcessEffects[index].Profile;
                usedProfile = target;
                enterTimer = 0f;
            }
        }
    }

    void UnSetEffect(ProfileID currentID)
    {
        tempWeight = volume.weight;
        if (usedProfile == currentID) { usedProfile = ProfileID.None; }
        exitTimer = 0f;
    }

    void EnterSnow()
    {
        SetEffect(ProfileID.Cold);
    }
    void ExitSnow()
    {
        UnSetEffect(ProfileID.Cold);
    }
    void EnterWarmth()
    {
        SetEffect(ProfileID.Warm);
    }
    void ExitWarmth()
    {
        UnSetEffect(ProfileID.Warm);
    }
}
