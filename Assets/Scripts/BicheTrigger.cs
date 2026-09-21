using UnityEngine;

public class BicheTrigger : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float speed;
    private bool move = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("Move", true);
            move = true;
        }
    }

    private void Update()
    {
        if (move)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }
}
