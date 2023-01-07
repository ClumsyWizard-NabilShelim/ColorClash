using System.Collections;
using UnityEngine;

public class StatusEffectVisualParticle : StatusEffectVisual
{
    [SerializeField] private GameObject activateEffect;
    [SerializeField] private GameObject deactivateEffect;
    [SerializeField] private GameObject effect;

    private void Start()
    {
        effect.SetActive(false);
    }

    public override void Activate()
    {
        Instantiate(activateEffect, transform.position, Quaternion.identity);
        effect.SetActive(true);
    }
    public override void Deactivate()
    {
        Instantiate(deactivateEffect, transform.position, Quaternion.identity);
        effect.SetActive(false);
    }
}