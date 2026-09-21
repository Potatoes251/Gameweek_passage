using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class InGameMenu : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField]
    private string      mainMenuScene;
    [SerializeField]
    private float       timeToClose;
    [SerializeField]
    private float       timeToOpen;
    [SerializeField]
    private GameObject  blurVolume;

    private Volume      blur;

    private float timer;

    private bool opened = false;
    private bool finishedMoving = true;

    private float baseHeigt;
    private float targetHeigt;

    private float timeToMove;
    private float openedHeight;
    private float closedHeight;
    void Start()
    {
        openedHeight = Screen.height * 2f;
        closedHeight = Screen.height * 0.5f;
        transform.position = new Vector3(Screen.width * 2f, openedHeight, 0f);

        blur = blurVolume.GetComponent<Volume>();
        blur.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.N) && finishedMoving)
        {
            if (opened) { CloseMenu(); }
            else        { OpenMenu(); }
        }

        if (!finishedMoving && timer < timeToMove)
        {
            timer += Time.deltaTime;

            transform.position = new Vector3(Screen.width / 2f, Mathf.Lerp(targetHeigt, baseHeigt, timer / timeToMove), 0f);
        }
        else
        {
            finishedMoving = true;
        }
    }

    private void OpenMenu()
    {
        timer = 0f;
        finishedMoving = false;
        opened = true;

        baseHeigt = closedHeight;
        targetHeigt = openedHeight;
        timeToMove = timeToOpen;

        blur.enabled = true;
    }
    private void CloseMenu()
    {
        timer = 0f;
        finishedMoving = false;
        opened = false;

        baseHeigt = openedHeight;
        targetHeigt = closedHeight;
        timeToMove = timeToClose;

        blur.enabled = false;
    }

    public void ContinueButtonClicked()
    {
        Debug.Log("Button pressed");


        if (opened) { CloseMenu(); }
    }
    public void BackToMenuButtonClicked()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
