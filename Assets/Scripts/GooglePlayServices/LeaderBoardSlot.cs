using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LeaderBoardSlot : MonoBehaviour
{
    [SerializeField] private RawImage icon;
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI nameText;

    public void Initialize(Texture2D iconTexture, int rank, string score, string name)
    {
        icon.texture = iconTexture;
        rankText.text = $"{rank} <sprite=7>";
        scoreText.text = $"Score: {score}";
        nameText.text = name;
    }
}
