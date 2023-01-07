using System.Collections;
using TMPro;
using UnityEngine;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentHighScore;

    private void Start()
    {
        if (PlayerDataManager.PlayerData.HighScore == 0)
            currentHighScore.transform.parent.gameObject.SetActive(false);
        else
            currentHighScore.text = $"<color=#FF5D5D>HighScore</color>\n {PlayerDataManager.PlayerData.HighScore}";
    }

    public void Play()
    {
        SceneManagement.Load("ArenaSelection");
    }
    public void Shop()
    {
        SceneManagement.Load("Shop");
    }
    public void Quit()
    {
        Application.Quit(); 
    }
}