using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [Header("Graphic")]
    [SerializeField]
    private Image background;
    [SerializeField]
    private Image staminaSlider;
    [SerializeField]
    private Image lostStaminaSlider;

    [Header("Stamina")]
    [SerializeField]
    private GameObject player;

    private PlayerStamina playerstamina;

    void Start()
    {
        if (player == null)             { Debug.LogWarning("Stamina bar is missing the player reference."); }
        if (background == null)         { Debug.LogWarning("Stamina bar is missing the background reference."); }
        if (staminaSlider == null)      { Debug.LogWarning("Stamina bar is missing the stamina slider reference."); }
        if (lostStaminaSlider == null)  { Debug.LogWarning("Stamina bar is missing the loststamina slider reference."); }

        playerstamina = player.GetComponent<PlayerStamina>();
    }

    void Update()
    {
        //widths:
        staminaSlider.fillAmount = (playerstamina.GetStamina() / playerstamina.GetMaxStamina()); ;

        lostStaminaSlider.fillAmount = (playerstamina.GetLostStamina() / playerstamina.GetMaxStamina());
    }
}
