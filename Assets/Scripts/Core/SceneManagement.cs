using ClumsyWizard.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : Persistant<SceneManagement>
{
    private AudioManager audioManager;
    private Animator animator;
    public static Action OnLoadTriggered;
    public static Action OnLoadTriggeredCore;
    public static Action OnNewSceneLoaded;
    public static Action OnNewSceneLoadedCore;
    public static bool isGameScene;
    public static bool isMainMenu;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        animator.SetBool("Fade", false);
        isGameScene = SceneManager.GetActiveScene().name.Contains("Level_");
        isMainMenu = SceneManager.GetActiveScene().name.Contains("MainMenu");
        audioManager = GetComponent<AudioManager>();

        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        if (!isGameScene)
            audioManager.Play("Background");
    }

    public static void Load(string levelString)
    {
        Instance.LoadLevel(levelString);
    }

    private void LoadLevel(string levelString)
    {
        StartCoroutine(LoadAsync(levelString));
    }

    public static void Reload()
    {
        string levelName = SceneManager.GetActiveScene().name;
        Instance.LoadLevel(levelName);
    }

    private IEnumerator LoadAsync(string levelString)
    {
        OnLoadTriggeredCore?.Invoke();
        OnLoadTriggered?.Invoke();
        OnLoadTriggered = null;
        animator.SetBool("Fade", true);
        yield return new WaitForSeconds(0.5f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelString);

        while (!operation.isDone)
        {
            yield return null;
        }
        isGameScene = levelString.Contains("Level_");
        isMainMenu = levelString.Contains("MainMenu");

        if (!isGameScene)
            audioManager.Play("Background");
        else
            audioManager.Stop("Background");

        animator.SetBool("Fade", false);
        OnNewSceneLoadedCore?.Invoke();
        OnNewSceneLoaded?.Invoke();
        OnNewSceneLoaded = null;
    }

    protected override void CleanUp()
    {

    }
}