using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DestroySelfOnContact : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(this.gameObject);
    }
}