using ClumsyWizard.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBlocker : Singleton<ScreenBlocker>
{
    [SerializeField] private Transform[] blockPoints;

    public static void Block(GameObject blockEffect)
    {
        int index = Random.Range(0, Instance.blockPoints.Length);
        Instantiate(blockEffect, Instance.blockPoints[index]);
    }
}
