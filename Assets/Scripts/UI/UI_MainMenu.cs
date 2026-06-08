using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    public void PlayBTN()
    {
        GameManager.instance.ContinuePlay();
    }

    public void QuitBTN()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
