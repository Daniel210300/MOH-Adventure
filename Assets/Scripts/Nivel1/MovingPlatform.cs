using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 moveOffset = new Vector3(0, 3, 0); 
    public float speed = 2f;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool goingUp = true;

    void Start()
    {
        startPos = transform.position;
        endPos = startPos + moveOffset;
    }

    void Update()
    {
        if (goingUp)
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, endPos) < 0.05f)
            goingUp = false;

        if (Vector3.Distance(transform.position, startPos) < 0.05f)
            goingUp = true;
    }
}
