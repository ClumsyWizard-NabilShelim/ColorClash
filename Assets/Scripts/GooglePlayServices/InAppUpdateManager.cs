using ClumsyWizard.Utilities;
using Google.Play.AppUpdate;
using Google.Play.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAppUpdateManager : Singleton<InAppUpdateManager>
{
    private AppUpdateManager appUpdateManager;
    [SerializeField] private GameObject inputBlocker;

    protected override void Awake()
    {
        base.Awake();
        inputBlocker.SetActive(true);
        appUpdateManager = new AppUpdateManager();
        StartCoroutine(CheckForUpdate());
    }

    private IEnumerator CheckForUpdate()
    {
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation = appUpdateManager.GetAppUpdateInfo();
        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful)
        {
            AppUpdateInfo updateInfo = appUpdateInfoOperation.GetResult();
            if(updateInfo.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                AppUpdateOptions appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();
                StartCoroutine(StartImmediateUpdate(updateInfo, appUpdateOptions));
            }
            else
            {
                inputBlocker.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Update check failed");
            inputBlocker.SetActive(false);
        }
    }

    private IEnumerator StartImmediateUpdate(AppUpdateInfo updateInfo, AppUpdateOptions appUpdateOptions)
    {
        var startUpdateRequest = appUpdateManager.StartUpdate(
          updateInfo,
          appUpdateOptions);
        yield return startUpdateRequest;

        inputBlocker.SetActive(false);
        Debug.Log("Failed to Update");
    }
}
