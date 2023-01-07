using ClumsyWizard.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColorSetter : MonoBehaviour
{
    [SerializeField] private Color color;

    private void Awake()
    {
        CameraHelper.Camera.backgroundColor = color;
    }
}
