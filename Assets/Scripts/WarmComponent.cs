using UnityEngine;

public class WarmComponent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameEvents.instance.EnterWarmth();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameEvents.instance.ExitWarmth();
        }
    }
}
