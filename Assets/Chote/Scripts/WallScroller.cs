using UnityEngine;
using UnityEngine.UI;

public class WallScroller : MonoBehaviour
{
    [SerializeField] private RawImage img;
    public float scrollFactor = 20f;

    PlayerMovement player;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>();
    }

    private void Update()
    {
        img.uvRect = new Rect(new Vector2(0,player.transform.position.y/scrollFactor), img.uvRect.size);
    }
}
