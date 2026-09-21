using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;

public class MainMenu : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField]
    private string gameScene;

    private EventInstance menuMusic;

    void Start()
    {
        menuMusic = RuntimeManager.CreateInstance("event:/MUSIC/Music_Menu");
        menuMusic.start();
    }

    public void PlayButtonClicked()
    {
        menuMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        menuMusic.release();

        SceneManager.LoadScene(gameScene);
    }

    public void LeaveButtonClicked()
    {
        Application.Quit();
    }

    public void OptionButtonClicked()
    {

    }
}