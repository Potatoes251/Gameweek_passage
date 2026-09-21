using UnityEngine;

public class credit : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField]
    private float timeToClose;
    [SerializeField]
    private float timeToOpen;
    [SerializeField, Range(0f, 1f)]
    private float openedWidth = 0.85f;
    [SerializeField, Range(1f, 2f)]
    private float closedWidth = 1.2f;
    [SerializeField, Range(0f, 1f)]
    private float height = 0.2f;

    private float timer;

    private bool opened = false;
    private bool finishedMoving = true;

    private float baseWidth;
    private float targetWidth;

    private float timeToMove;

    void Start()
    {
        openedWidth *= Screen.width;
        closedWidth *= Screen.width;
        transform.position = new Vector3(closedWidth, Screen.height * height, 0f);
    }

    void Update()
    {
        if (!finishedMoving && timer < timeToMove)
        {
            timer += Time.deltaTime;

            transform.position = new Vector3(Mathf.Lerp(targetWidth, baseWidth, timer / timeToMove), Screen.height * height, 0f);
        }
        else
        {
            finishedMoving = true;
        }
    }

    private void OpenCredit()
    {
        timer = 0f;
        finishedMoving = false;
        opened = true;

        baseWidth = openedWidth;
        targetWidth = closedWidth;
        timeToMove = timeToOpen;
    }
    private void CloseCredit()
    {
        timer = 0f;
        finishedMoving = false;
        opened = false;

        baseWidth = closedWidth;
        targetWidth = openedWidth;
        timeToMove = timeToClose;
    }

    public void SwitchState()
    {
        if (finishedMoving)
        {
            if (opened) { CloseCredit(); }
            else { OpenCredit(); }
        }
    }
}