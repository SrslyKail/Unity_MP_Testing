using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Pickup<T> : NetworkBehaviour 
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    protected bool IsCollected;
    public abstract T Collect();
    
    protected void Show(bool show)
    {
        spriteRenderer.enabled = show;
    }
}
