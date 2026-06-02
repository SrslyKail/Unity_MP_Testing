using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField] private float lifetimeSeconds = 1f;

    private void Start()
    {
        Destroy(this.gameObject, lifetimeSeconds);
    }
}