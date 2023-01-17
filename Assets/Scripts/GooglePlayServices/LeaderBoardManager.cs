using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardManager : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotHolder;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Show(Dictionary<string, LeaderBoardData> data)
    {
        if(slotHolder.childCount > 0)
        {
            for (int i = 0; i < slotHolder.childCount; i++)
            {
                Destroy(slotHolder.GetChild(i).gameObject);
            }
        }

        animator.SetBool("Show", true);

        foreach (string userID in data.Keys)
        {
            LeaderBoardData dataValue = data[userID];        
            Instantiate(slotPrefab, slotHolder).GetComponent<LeaderBoardSlot>().Initialize(dataValue.UserProfile.image, dataValue.Rank, dataValue.Score, dataValue.UserProfile.userName);
        }
    }
    
    public void Close()
    {
        animator.SetBool("Show", false);
    }
}
