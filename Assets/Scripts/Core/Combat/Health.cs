using System;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    
    [NonSerialized]
    public NetworkVariable<int> CurrentHealth = new ();
    
    private bool _isDead = false;

    public Action<Health> OnDeath;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }
        
        CurrentHealth.Value = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        ModifyHealth(-damage);
    }

    public void RestoreHealth(int heal)
    {
        ModifyHealth(heal);
    }

    private void ModifyHealth(int value)
    {
        if (_isDead)
        {
            return;
        }
        int newHealth = CurrentHealth.Value + value;
        CurrentHealth.Value = Mathf.Clamp(newHealth, 0, MaxHealth);
        
        if (CurrentHealth.Value == 0)
        {
            OnDeath?.Invoke(this);
            _isDead = true;
        }
    }
}
