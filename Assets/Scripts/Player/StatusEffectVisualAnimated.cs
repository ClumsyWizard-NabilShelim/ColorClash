using System.Collections;
using UnityEngine;

public class StatusEffectVisualAnimated : StatusEffectVisual
{
    private Animator animator;
    private AudioManager audioManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioManager = GetComponent<AudioManager>();
    }

    public override void Activate()
    {
        audioManager.Play("Activate");
        animator.SetBool("Activate", true);
    }
    public override void Deactivate()
    {
        audioManager.Play("Deactivate");
        animator.SetBool("Activate", false);
    }
}