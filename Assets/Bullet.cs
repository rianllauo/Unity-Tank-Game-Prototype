using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
