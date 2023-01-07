using System.Collections;
using UnityEngine;

public class ScreenBlock : MonoBehaviour
{
    [SerializeField] private float duration;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        Invoke("Remove", duration);
    }

    private void Remove()
    {
        animator.SetTrigger("Remove");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}