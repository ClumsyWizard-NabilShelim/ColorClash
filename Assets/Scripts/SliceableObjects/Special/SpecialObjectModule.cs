using System.Collections;
using UnityEngine;

public abstract class SpecialObjectModule : MonoBehaviour
{
    [SerializeField] protected float effectDuration;
    public abstract void UsePowerUp();
}