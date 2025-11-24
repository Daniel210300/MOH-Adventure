using UnityEngine;

public class DoorRamas : MonoBehaviour
{
    public Animator animator;

    public void OpenDoor()
    {
        if (animator != null)
        {
            animator.SetTrigger("Open");
        }
    }
}
