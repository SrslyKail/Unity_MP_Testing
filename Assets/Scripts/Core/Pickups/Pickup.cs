using System;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Pickup<T> : NetworkBehaviour 
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    protected bool IsCollected;
    public abstract T Collect();
    public abstract event Action<Pickup<T>> OnCollected;
    
    protected void Show(bool show)
    {
        spriteRenderer.enabled = show;
    }

    public abstract void Reset();
}
