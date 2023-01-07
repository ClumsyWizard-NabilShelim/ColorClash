using System.Collections;
using TMPro;
using UnityEngine;

public class VisualFeedback : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Animator animator;
    private bool showing;
    [field: SerializeField] public bool LowPriority { get; private set; }
    public bool pulse;

    public void Show(string text, float duration)
    {
        this.text.text = text;

        if (showing)
            animator.SetTrigger("Reset");

        animator.SetBool("PopIn", true);
        
        if(pulse)
            animator.SetBool("Pulse", true);

        CancelInvoke("Close");
        Invoke("Close", duration);
        showing = true;
    }

    public void Close()
    {
        showing = false;
        CancelInvoke("Close");
        animator.SetBool("PopIn", false);
        animator.SetBool("Pulse", false);
    }

    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}