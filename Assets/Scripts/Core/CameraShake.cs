using ClumsyWizard.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShakeIntensity
{
    Low,
    Medium,
    High,
    Ultra
}

public class CameraShake : Singleton<CameraShake>
{
    private bool shaking;
    private float magnitude;
    private float currentTime;
    private Vector3 origin;
    private new Transform transform;
    private bool canShake;

    [SerializeField] private float smoothing;
    [SerializeField] private int maxRotationAngle;

    protected override void Awake()
    {
        base.Awake();
        transform = CameraHelper.Camera.transform;
        origin = transform.position;

        SceneManagement.OnNewSceneLoadedCore += () =>
        {
            shaking = false;
            transform.position = origin;
            transform.rotation = Quaternion.identity;
        };
    }

    private void Update()
    {
        if (!canShake)
            return;

        if (!shaking)
            return;

        if (currentTime > 0)
        {
            Vector2 randomPos = Random.insideUnitCircle;
            transform.position = Vector3.Lerp(transform.position, new Vector3(magnitude * randomPos.x, magnitude * randomPos.y, transform.position.z), Time.deltaTime * smoothing);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, Random.Range(-maxRotationAngle, maxRotationAngle)), Time.deltaTime * smoothing);
            currentTime -= Time.deltaTime;
        }
        else
        {
            shaking = false;
            transform.position = origin;
            transform.rotation = Quaternion.identity;
        }
    }

    public static void Shake(ShakeIntensity intensity)
    {
        if (!Instance.canShake)
            return;

        if (Instance.shaking)
            return;

        switch (intensity)
        {
            case ShakeIntensity.Low:
                Instance.currentTime = 0.1f;
                Instance.magnitude = 0.2f;
                break;
            case ShakeIntensity.Medium:
                Instance.currentTime = 0.2f;
                Instance.magnitude = 0.35f;
                break;
            case ShakeIntensity.High:
                Instance.currentTime = 0.3f;
                Instance.magnitude = 0.6f;
                break;
            case ShakeIntensity.Ultra:
                Instance.currentTime = 0.6f;
                Instance.magnitude = 1.0f;
                break;
            default:
                break;
        }

        Instance.shaking = true;
    }

    public static void CanShake(bool active)
    {
        Instance.canShake = active;
    }
}
