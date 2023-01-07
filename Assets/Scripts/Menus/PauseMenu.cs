using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        SceneManagement.OnLoadTriggered += () =>
        {
            UnPause();
        };
    }

    public void Pause()
    {
        animator.SetBool("Show", true);
        Time.timeScale = 0;
        GameManager.IsPaused = true;
    }
    public void UnPause()
    {
        animator.SetBool("Show", false);
        Time.timeScale = 1.0f;
        GameManager.IsPaused = false;
    }

    public void Settings()
    {
        SettingsMenu.Open();
    }
    public void MainMenu()
    {
        SceneManagement.Load("MainMenu");
    }
    public void Quit()
    {
        Application.Quit();
    }
}