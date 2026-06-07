using UnityEngine;
using UnityEngine.UI;

public class UI_Options : MonoBehaviour
{
    private Player player;
    [SerializeField] private Toggle healthBarToggle;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
    
        healthBarToggle.onValueChanged.AddListener(OnHealthBarToggleChanged);
    }


    private void OnHealthBarToggleChanged(bool isOn)
    {
        player.entityHealth.EnableHealthBar(isOn);
    }
}
