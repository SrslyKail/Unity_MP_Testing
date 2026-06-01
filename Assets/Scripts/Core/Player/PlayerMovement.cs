using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Rigidbody2D rb;
    [Header("Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float turnRateDegPerSecond;

    private Vector2 _previousMovementInput;
    
    //Not using Start so we ensure we don't access data until networking is set up
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return; 
        }

        inputReader.MoveEvent += HandleMove;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return; 
        }
        
        inputReader.MoveEvent -= HandleMove;
        
    }

    private void HandleMove(Vector2 movementInput)
    {
        _previousMovementInput = movementInput;
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        //Multiplay by negative turn rate, otherwise controls are backwards
        float zRotation = _previousMovementInput.x * -turnRateDegPerSecond * Time.deltaTime;
        bodyTransform.Rotate(0f, 0f, zRotation);
    }

    private void FixedUpdate()
    {
        //Update RB here so we stay in sync with physics updates
        if (!IsOwner)
        {
            return;
        }
        //Apply forwards/backwards forces
        rb.linearVelocity = (Vector2)bodyTransform.up * (_previousMovementInput.y * movementSpeed);
    }
}
