using UnityEngine;

public class RepeatBG : MonoBehaviour
{
    Vector3 Startposition;
    float repeatHigh;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Startposition = transform.position;
        repeatHigh = GetComponent<BoxCollider2D>().size.y / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < Startposition.y - repeatHigh)
        {
            transform.position = Startposition;
        }
    }
}
