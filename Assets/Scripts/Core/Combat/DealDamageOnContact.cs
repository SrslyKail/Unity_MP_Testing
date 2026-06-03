using Unity.Netcode;
using UnityEngine;

public class DealDamageOnContact: MonoBehaviour
{
    [SerializeField] private int damageAmount = 5;

    private ulong _ownerClientId;
    
    public void SetOwner(ulong ownerClientId)
    {
        this._ownerClientId = ownerClientId;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Rigidbody2D otherRigidbody = otherCollider.attachedRigidbody;

        if (otherRigidbody == null)
        {
            return;
        }
        
        if (otherRigidbody.TryGetComponent(out NetworkObject otherObject))
        {
            if (otherObject.OwnerClientId == _ownerClientId)
            {
                Debug.Log("ignoring owner");
                return;
            }
        }
        
        if (otherRigidbody.TryGetComponent(out Health health))
        {
            Debug.Log($"dealing {damageAmount} damage");
            health.TakeDamage(damageAmount);
        }
    }
}
