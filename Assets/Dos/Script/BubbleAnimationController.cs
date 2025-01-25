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
    }
    public void GotPOP()
    {
        anim.SetTrigger("POP");
    }
    public void HideSprite()
    {
        anim.Play("BlubBlub", 0, 0);
        sr.enabled = false;
        anim.enabled = false;
    }
}
