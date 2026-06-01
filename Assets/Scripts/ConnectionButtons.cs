using Unity.Netcode;
using UnityEngine;

public class ConnectionButtons: MonoBehaviour
{
    public void JoinClient()
    {
        NetworkManager.Singleton.StartClient();
    }
    
    public void JoinHost()
    {
        NetworkManager.Singleton.StartHost();
    }
}
