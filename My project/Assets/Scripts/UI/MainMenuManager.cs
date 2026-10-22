using UnityEngine;
using UnityEngine.SceneManagement; // Wajib ditambahkan untuk pindah scene

public class MainMenuManager : MonoBehaviour
{
    public void OnPlayClicked()
    {
        // Pindah ke Scene Gameplay
        SceneManager.LoadScene("Gameplay");
    }

    public void OnQuitClicked()
    {
        Debug.Log("Keluar dari game...");
        Application.Quit();
    }
}