using Unity.Netcode;
using UnityEngine;

public class PlayerAiming : NetworkBehaviour
{
    [SerializeField] private Transform turretTransform;
    [SerializeField] private InputReader inputReader;

    //Late update so we move tank, then calculate rotations.
    private void LateUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        
        //[?] Should we cache the main camera to avoid the expensive Camera.main?
        Vector2 aimWorldPosition= Camera.main?
            .ScreenToWorldPoint(inputReader.AimPosition)
                                  ?? Vector3.zero;

        turretTransform.up = new Vector2(
            aimWorldPosition.x - turretTransform.position.x,
            aimWorldPosition.y - turretTransform.position.y
        );

    }
}
