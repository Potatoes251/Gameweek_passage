using UnityEngine;

public class Wind : MonoBehaviour
{
    public Vector3 direction = Vector3.forward;
    public float strength = 1.0f;

    void Start()
    {
        direction.Normalize();
        strength *= Time.fixedDeltaTime;
    }
}
