using UnityEngine;

public class DestoryByTime : MonoBehaviour
{
    [SerializeField]
    private float lifetime;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
