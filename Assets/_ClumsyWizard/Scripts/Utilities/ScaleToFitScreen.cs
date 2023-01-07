using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToFitScreen : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public bool scale;

    private void Start()
    {
        Scale();
    }

    private void OnValidate()
    {
        if(scale)
        {
            scale = false;
            Scale();
        }    
    }

    private void Scale()
    {
        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight * Camera.main.aspect;
        float spriteWidth = worldScreenWidth / spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = worldScreenHeight / spriteRenderer.sprite.bounds.size.y;
        transform.localScale = new Vector3(spriteHeight, spriteHeight, 1);
        if ((spriteRenderer.sprite.bounds.size.x - spriteRenderer.sprite.bounds.size.y) <= 0.1f)
            spriteRenderer.size = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);
        else
            spriteRenderer.size = new Vector2(spriteRenderer.sprite.bounds.size.x / spriteWidth, spriteRenderer.sprite.bounds.size.y);
    }
}
