using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [SerializeField] private Transform slotHolder;
    [SerializeField] private GameObject arenaSlot;

    private void Start()
    {
        AssetLoader.LoadAssetsByTag("Arena", (List<ArenaShopItemData> data) =>
        {
            data.Sort((ArenaShopItemData a, ArenaShopItemData b) =>
            {
                return a.Index.CompareTo(b.Index);
            });
            for (int i = 0; i < data.Count; i++)
            {
                Instantiate(arenaSlot, slotHolder).GetComponent<ArenaSlot>().Initialize(data[i]);
            }
        });

        AdsManager.GetAd(AdType.Banner, (BannerAds ad) =>
        {
            ad.Setup(() =>
            {
                ad.Show();
            },
            UnityEngine.Advertisements.BannerPosition.BOTTOM_CENTER);
        }, null);
    }
    
    public void Back()
    {
        SceneManagement.Load("MainMenu");
    }
}
