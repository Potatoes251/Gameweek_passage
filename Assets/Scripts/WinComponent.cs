using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinComponent : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField]
    private RawImage Screen1;
    [SerializeField]
    private RawImage Screen2;
    [SerializeField]
    private float Screen1Duration;
    [SerializeField]
    private float Screen2Duration;



    private float   timer = 0;
    private bool    won = false;

    void Start()
    {
        Screen1.enabled = false;
        Screen2.enabled = false;
    }

    void Update()
    {
        if (won)
        {
            timer += Time.deltaTime;

            if (timer > Screen1Duration + Screen2Duration)
            {
                SceneManager.LoadScene("Main Menu Scene");
            }
            else if (timer > Screen1Duration)
            {
                Screen1.enabled = false;
                Screen2.enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        won = true;

        Screen1.enabled = true;
        Screen2.enabled = false;
    }
}
