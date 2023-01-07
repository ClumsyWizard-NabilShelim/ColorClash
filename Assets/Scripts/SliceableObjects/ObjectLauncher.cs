using ClumsyWizard.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ObjectColor
{
    red,
    green,
    blue,
    purple
}

[Serializable]
public class SliceableObjectData
{
    public GameObject SliceableObject;
    public int Coin;
    public int SpawnChance;
}

[Serializable]
public class ShapeSliceableObjectData : SliceableObjectData
{
    public Color Color;
    public Sprite Sprite;
}

[Serializable]
public class SpecialSliceableObjectData  : SliceableObjectData
{}
[Serializable]
public class EnemySliceableObjectData : SliceableObjectData
{}


public class ObjectLauncher : Singleton<ObjectLauncher>
{
    private AudioManager audioManager;
    public static bool SuccessfullSlice;
    public static Action<List<SliceableObject>> OnObjectsLaunched;

    [Header("Spawning Objects")]
    [SerializeField] private int spreadAngle;
    public static List<SliceableObject> LaunchedSliceableObject { get; private set; } = new List<SliceableObject>();
    private List<Vector2> spawnPoints;
    private List<Vector2> targetPoints;

    [Header("Shape Object")]
    [SerializeField] private ClumsyDictionary<ObjectColor, ShapeSliceableObjectData> shapeObjects;
    [SerializeField] private GameObject spawnObject;
    [SerializeField] private float spawnInterval;
    [SerializeField] private int spawnCount;

    [Header("Special Object")]
    [SerializeField] private List<SpecialSliceableObjectData> specialObjects;

    [Header("Enemy Object")]
    [SerializeField] private List<EnemySliceableObjectData> enemyObjects;

    private void Start()
    {
        audioManager = GetComponent<AudioManager>();
        InitializeSpawnPoints();
        InitializeTargetPoints();
        Invoke("DelayedSpawnObject", 2);
    }

    private void InitializeSpawnPoints()
    {
        float width = CameraViewportHandler.Width;
        float increment = (width / spawnCount);
        Vector2 startPos = (Vector2)transform.position - new Vector2(width / 2, 0);
        spawnPoints = new List<Vector2>();
        for (int i = 0; i < spawnCount; i++)
        {
            spawnPoints.Add(startPos + new Vector2(i * increment + (increment / 2.0f), 0));
        }
    }

    private void InitializeTargetPoints()
    {
        float width = CameraViewportHandler.Width;
        float increment = (width / spawnCount);
        Vector2 startPos = Vector2.zero - new Vector2(width / 2, 0);
        targetPoints = new List<Vector2>();
        for (int i = 0; i < spawnCount; i++)
        {
            targetPoints.Add(startPos + new Vector2(i * increment + (increment / 2.0f), 0));
        }
    }

    private void DelayedSpawnObject()
    {
        SuccessfullSlice = true;
        StartCoroutine(SpawnObjects());
    }

    private void CheckIfSliceMissed()
    {
        if (!SuccessfullSlice)
            PlayerManager.SliceMissed();
    }

    private IEnumerator SpawnObjects()
    {
        LaunchedSliceableObject.Clear();

        SuccessfullSlice = false;

        audioManager.Play("Launch");
        LaunchShapeObjects();
        yield return new WaitForSeconds(0.1f);
        LaunchSpecialObjects();
        yield return new WaitForSeconds(0.1f);
        LaunchEnemyObjects();

        //The 0.5f is a magic number!
        Invoke("CheckIfSliceMissed", 0.5f + GameManager.UnFreezeDelay);

        OnObjectsLaunched?.Invoke(LaunchedSliceableObject);

        yield return new WaitForSeconds(spawnInterval);

        if (!GameManager.IsGameOver)
            StartCoroutine(SpawnObjects());
    }

    //Shape Objects
    private void LaunchShapeObjects()
    {
        List<Vector2> buffer = spawnPoints.ToList();
        List<Vector2> targetPointBuffer = targetPoints.ToList();
        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, buffer.Count);

            ShapeSliceableObject sliceableObject = Instantiate(spawnObject, buffer[randomIndex], Quaternion.Euler(0, 0, UnityEngine.Random.Range(-spreadAngle, spreadAngle))).GetComponent<ShapeSliceableObject>();
            LaunchedSliceableObject.Add(sliceableObject);

            ObjectColor color = shapeObjects.Keys[UnityEngine.Random.Range(0, shapeObjects.Keys.Count)];
            sliceableObject.Initialize(color, shapeObjects[color], targetPointBuffer[randomIndex]);

            buffer.RemoveAt(randomIndex);
            targetPointBuffer.RemoveAt(randomIndex);
        }
    }
    //Special Objects
    private void LaunchSpecialObjects()
    {
        int change = UnityEngine.Random.Range(0, 101);
        int index = UnityEngine.Random.Range(0, specialObjects.Count);
        if (specialObjects[index].SpawnChance >= change)
        {
            int spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            SpecialSliceableObject sliceableObject = Instantiate(specialObjects[index].SliceableObject, spawnPoints[spawnPointIndex], Quaternion.Euler(0, 0, UnityEngine.Random.Range(-spreadAngle, spreadAngle))).GetComponent<SpecialSliceableObject>();
            sliceableObject.Initialize(specialObjects[index], targetPoints[spawnPointIndex]);
            LaunchedSliceableObject.Add(sliceableObject);
        }
    }

    //Enemy Objects
    private void LaunchEnemyObjects()
    {
        int change = UnityEngine.Random.Range(0, 101);
        int index = UnityEngine.Random.Range(0, enemyObjects.Count);
        if (enemyObjects[index].SpawnChance >= change)
        {
            int spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            EnemySliceableObject sliceableObject = Instantiate(enemyObjects[index].SliceableObject, spawnPoints[spawnPointIndex], Quaternion.Euler(0, 0, UnityEngine.Random.Range(-spreadAngle, spreadAngle))).GetComponent<EnemySliceableObject>();
            sliceableObject.Initialize(enemyObjects[index], targetPoints[spawnPointIndex]);
            LaunchedSliceableObject.Add(sliceableObject);
        }
    }

    //Helper
    public static ShapeSliceableObjectData GetShapeData(ObjectColor color)
    {
        return Instance.shapeObjects[color];
    }
    private void OnDrawGizmos()
    {
        float width = CameraViewportHandler.Width;
        float increment = (width / spawnCount);
        Vector2 startPos = (Vector2)transform.position - new Vector2(width / 2, 0);
        Gizmos.color = Color.red;
        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 spawnPoint = startPos + new Vector2(i * increment + (increment / 2.0f), 0);
            Gizmos.DrawLine(spawnPoint, spawnPoint + CWMath.GetDirectionFromAngle(spreadAngle) * 5);
            Gizmos.DrawLine(spawnPoint, spawnPoint + CWMath.GetDirectionFromAngle(-spreadAngle) * 5);
            Gizmos.DrawSphere(spawnPoint, 0.1f);
            Gizmos.DrawSphere(new Vector2(spawnPoint.x, 0), 0.1f);
        }
    }
}