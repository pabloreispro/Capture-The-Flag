using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Networking;

public class RedFlag : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(IsOwner)
        {
            if(other.tag == "Blue")
        {
             
            var player = other.gameObject.GetComponent<PlayerNetwork>();
            player.FlagPlayerServerRpc();
            DisableFlagServerRpc();
        }
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
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

