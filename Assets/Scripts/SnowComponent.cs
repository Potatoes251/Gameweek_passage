using UnityEngine;

public class SnowComponent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameEvents.instance.EnterSnow();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameEvents.instance.ExitSnow();
        }
    }
}
