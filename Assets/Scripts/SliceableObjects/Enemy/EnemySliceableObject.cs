using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySliceableObject : SliceableObject
{
    private enum EnemyState
    {
        Normal,
        Sliced,
        AIActivationComplete,
    }

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite deathSprite;
    [SerializeField] private VisualFeedback feedback;
    private Animator animator;
    private EnemyAIModule enemyAI;
    private EnemyState state;
    private bool dead;

    public override void Initialize(SliceableObjectData data, Vector2 targetPos)
    {
        base.Initialize(data, targetPos);
        enemyAI = GetComponent<EnemyAIModule>();
        animator = GetComponent<Animator>();
        state = EnemyState.Normal;
    }

    public override void Slice()
    {
        if (dead)
            return;

        base.Slice();
    }

    protected override void Fall()
    {
        if (state == EnemyState.Sliced)
        {
            base.Fall();
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Static;
            animator.SetBool("Activate", true);
        }
    }

    private void ActivateAI()
    {
        if (state == EnemyState.Sliced)
        {
            animator.SetBool("Activate", false);
            rb.bodyType = RigidbodyType2D.Dynamic;
            base.Fall();
            return;
        }
        enemyAI.Activate();
        state = EnemyState.AIActivationComplete;
        Slice();
    }

    protected override void OnRemove()
    {
        dead = true;
        if (state == EnemyState.AIActivationComplete)
        {
            if (feedback != null)
            {
                feedback.gameObject.SetActive(true);
                feedback.transform.SetParent(null);
                feedback.transform.position = transform.position;
                feedback.Show(enemyAI.FeedbackText, 2);
                Destroy(feedback.gameObject, 5);
            }
            base.OnRemove();
        }
        else
        {
            state = EnemyState.Sliced;
            spriteRenderer.sprite = deathSprite;
            Destroy(gameObject, 10.0f);
        }
    }

    protected override void SliceActions()
    {
        CameraShake.Shake(ShakeIntensity.Medium);
        Effects(false);
    }
}
