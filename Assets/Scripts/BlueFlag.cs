using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;

public class BlueFlag : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
            if(other.tag == "Red")
            {
                
                var player = other.gameObject.GetComponent<PlayerNetwork>();
                player.FlagPlayerServerRpc();
                DisableFlagServerRpc();
            }
        
        
    }

    [ServerRpc (RequireOwnership = false)]
    private void DisableFlagServerRpc()
    {
        DisableFlagClientRpc();
    }

    [ClientRpc]
    private void DisableFlagClientRpc()
    {
        gameObject.SetActive(false);
    }
}
