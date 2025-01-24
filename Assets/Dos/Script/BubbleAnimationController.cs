using UnityEngine;

public class BubbleAnimationController : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        anim.enabled = false;
    }
    public void StartBubbleAnimation()
    {
        anim.enabled = true;
        Debug.LogWarning("GOT CALLED");
    }
    public void GotPOP()
    {
        anim.SetTrigger("POP");
        Debug.LogWarning("GOT POP");
    }
    public void HideSprite()
    {
        sr.enabled = false;
        anim.enabled = false;
    }
}
