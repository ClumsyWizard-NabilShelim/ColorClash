using ClumsyWizard.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAutoCameraAssigner : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = CameraHelper.Camera;
    }
}
