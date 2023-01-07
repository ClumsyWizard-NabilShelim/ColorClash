using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SliceableObjectType
{
    Color,
    PowerUp,
    Enemy
}

public abstract class SliceableObject : MonoBehaviour
{
    public int Coin { get; protected set; }

    [SerializeField] protected Rigidbody2D rb;
    private BoxCollider2D collider;
    [field: SerializeField] public SliceableObjectType Type { get; private set; }

    [Header("Stats")]
    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;

    [Header("Visuals")]
    [SerializeField] private GameObject particleEffect;
    private float defaultGravityScale;
    private bool launch;
    private bool isFrozen;

    public virtual void Initialize(SliceableObjectData data, Vector2 targetPos)
    {
        collider = GetComponent<BoxCollider2D>();
        Coin = data.Coin;
        float timeToDestination = 1.5f;
        float yForce = (Mathf.Abs(targetPos.y - transform.position.y) + (0.5f * rb.gravityScale * 9.8f * timeToDestination * timeToDestination)) / timeToDestination;
        rb.AddForce(transform.up * (yForce + Random.Range(-minForce, maxForce)), ForceMode2D.Impulse);
        defaultGravityScale = rb.gravityScale;
        launch = true;

        Destroy(gameObject, 10.0f);
    }

    private void Update()
    {
        if (!launch)
            return;

        if (rb.velocity.y < 0.1f)
        {
            launch = false;
            Stop();
            StartCoroutine(FallDelayed());
        }
    }

    private IEnumerator FallDelayed()
    {
        yield return new WaitForSeconds(GameManager.UnFreezeDelay);
        collider.isTrigger = true;
        Fall();
    }

    private void Stop()
    {
        rb.gravityScale = 0.0f;
    }
    protected virtual void Fall()
    {
        if (isFrozen)
            return;

        rb.gravityScale = defaultGravityScale * GameManager.FallGravityMultiplier;
    }
    public void Freeze()
    {
        isFrozen = true;
    }
    public void UnFreeze()
    {
        isFrozen = false;
        Fall();
    }
    public virtual void Slice()
    {
        SliceActions();
        OnRemove();
    }
    protected virtual void OnRemove()
    {
        Destroy(gameObject);
    }
    protected abstract void SliceActions();

    protected void Effects(bool adjustColor, Color color = default)
    {
        GameObject effect = Instantiate(particleEffect, transform.position, Quaternion.identity);

        if (adjustColor)
        {
            ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = particleSystem.main;
            main.startColor = color;
        }
    }
}
