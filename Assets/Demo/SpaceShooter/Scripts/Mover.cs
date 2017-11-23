using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.velocity = transform.forward * speed;
    }
}
