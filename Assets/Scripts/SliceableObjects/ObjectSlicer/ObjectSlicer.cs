using ClumsyWizard.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ObjectSlicer : Singleton<ObjectSlicer>
{
    private AudioManager audioManager;
    private bool isCutting;
    private List<SliceableObject> slicedObjects = new List<SliceableObject>();
    private Action<List<SliceableObject>> onObjectsSliced;
    [SerializeField] private GameObject bladePrefab;
    private Blade[] blades;
    [SerializeField] private int minSliceChainCount;
    [SerializeField] private Animator wrongSliceMarker;
    public static int EnemiesKilled { get; private set; }

    private void Start()
    {
        audioManager = GetComponent<AudioManager>();
        blades = new Blade[PlayerDataManager.PlayerStats.Blades];
        for (int i = 0; i < PlayerDataManager.PlayerStats.Blades; i++)
        {
            blades[i] = Instantiate(bladePrefab, transform).GetComponent<Blade>();
            blades[i].Initialize(OnSlice);
        }

        InputManager.OnSignificantPointerMove(() =>
        {
            audioManager.Play("Slice");
        });
    }

    void Update()
    {
        if (InputManager.IsPointerMoving)
        {
            isCutting = true;
        }
        else
        {
            if (isCutting)
                ComputeSlice();

            isCutting = false;
        }

        if (isCutting)
        {
            for (int i = 0; i < Mathf.Min(InputManager.PointerWorldPositions.Count, blades.Length); i++)
            {
                blades[i].transform.position = InputManager.PointerWorldPositions[i];
                blades[i].Slice();
            }
        }
        else
        {
            for (int i = 0; i < blades.Length; i++)
            {
                blades[i].StopSlice();
            }
        }
    }

    private List<ShapeSliceableObject> objectsToSlice = new List<ShapeSliceableObject>();
    private void ComputeSlice()
    {
        if (slicedObjects.Count > 0)
        {
            onObjectsSliced?.Invoke(slicedObjects.ToList());

            if (slicedObjects[0].Type == SliceableObjectType.PowerUp)
                PowerUpSlice();
            else if (slicedObjects[0].Type == SliceableObjectType.Enemy)
                EnemySlice();
            else
                ShapeSlice();

            slicedObjects.Clear();
            objectsToSlice.Clear();
        }
    }

    private void PowerUpSlice()
    {
        slicedObjects[0].Slice();
        PlayerManager.GoodSlice(slicedObjects.Count, slicedObjects[0].Coin);
    }

    private void EnemySlice()
    {
        int sliceCount = 0;
        int coin = 0;
        for (int i = 0; i < slicedObjects.Count; i++)
        {
            if (slicedObjects[i].Type != SliceableObjectType.Enemy)
                continue;

            coin += slicedObjects[i].Coin;
            slicedObjects[i].Slice();
            sliceCount++;
        }
        PlayerManager.GoodSlice(sliceCount, coin);
    }

    private void ShapeSlice()
    {
        int colorsInRow = 1;
        ObjectColor targetColor = ((ShapeSliceableObject)slicedObjects[0]).Color;
        bool goodSlice = true;
        for (int i = 1; i < slicedObjects.Count; i++)
        {
            if (slicedObjects[i].Type != SliceableObjectType.Color)
                continue;

            ShapeSliceableObject slicedObject = (ShapeSliceableObject)slicedObjects[i];

            if (targetColor == slicedObject.Color)
            {
                if (!goodSlice)
                    continue;

                colorsInRow++;
                objectsToSlice.Add(slicedObject);
            }
            else
            {
                goodSlice = false;
                break;
            }
        }
        if (goodSlice)
        {
            if (colorsInRow >= minSliceChainCount)
            {
                PlayerManager.GoodSlice(colorsInRow, colorsInRow * slicedObjects[0].Coin);
                slicedObjects[0].Slice();
                for (int i = 0; i < objectsToSlice.Count; i++)
                {
                    objectsToSlice[i].Slice();
                }
            }
        }
        else
        {
            wrongSliceMarker.transform.position = slicedObjects[slicedObjects.Count - 1].transform.position;
            wrongSliceMarker.SetTrigger("Pop");
            PlayerManager.BadSlice();
        }
    }

    private void OnSlice(SliceableObject sliceableObject)
    {
        if (sliceableObject.Type == SliceableObjectType.Enemy)
            EnemiesKilled++;

        slicedObjects.Add(sliceableObject);
    }

    public static void OnObjectsSliced(Action<List<SliceableObject>> callback)
    {
        Instance.onObjectsSliced += callback;
    }
}
