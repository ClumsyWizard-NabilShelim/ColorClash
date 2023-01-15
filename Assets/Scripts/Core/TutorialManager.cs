using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;
    [SerializeField] private GameObject uiContainer;
    private int currentIndex;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        uiContainer.SetActive(false);
        SaveLoadManager.LoadData(SaveLoadKey.Tutorial, (string tutorialShown) =>
        {
            if (tutorialShown != default)
            {
                Destroy(gameObject);
            }
            else
            {
                SaveLoadManager.SaveData("Tutorial Shown", SaveLoadKey.Tutorial);
                uiContainer.SetActive(true);
                nextButton.gameObject.SetActive(true);
                closeButton.gameObject.SetActive(false);

                for (int i = 0; i < panels.Length; i++)
                {
                    panels[i].SetActive(false);
                }

                panels[currentIndex].SetActive(true);
                Time.timeScale = 0.0f;
            }
        });
    }

    public void Next()
    {
        panels[currentIndex].SetActive(false);

        if (currentIndex == panels.Length - 1)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }

        panels[currentIndex].SetActive(true);

        if (currentIndex == panels.Length - 1)
        {
            nextButton.interactable = false;
            closeButton.gameObject.SetActive(true);
        }
        else
        {
            nextButton.interactable = true;
            closeButton.gameObject.SetActive(false);
        }
    }
    public void Previous()
    {
        panels[currentIndex].SetActive(false);

        if (currentIndex == 0)
        {
            currentIndex = panels.Length - 1;
        }
        else
        {
            currentIndex--;
        }

        panels[currentIndex].SetActive(true);

        if (currentIndex == panels.Length - 1)
        {
            nextButton.interactable = false;
            closeButton.gameObject.SetActive(true);
        }
        else
        {
            nextButton.interactable = true;
            closeButton.gameObject.SetActive(false);
        }
    }

    public void Close()
    {
        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }
}
