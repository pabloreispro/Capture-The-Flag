using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class blueBase : NetworkBehaviour
{
   public GameObject redFlag;
    
    private void Start ()
    {
        GameManager.instance.scoreB.OnValueChanged += (a, b) =>  GameManager.instance.scoreBlue.text = b.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Blue" && other.GetComponent<PlayerNetwork>().getFlag.Value == true)
        {
            other.gameObject.transform.position = GameManager.instance.spawnBlue.position;
            var player = other.gameObject.GetComponent<PlayerNetwork>();
            player.DisableFlagPlayerServerRpc();
            JServerRpc();
            
            
        }
    }

    
    [ServerRpc(RequireOwnership = false)]
    private void JServerRpc()
    {
        
        var g = GameManager.instance.scoreB.Value += 1;
        
        JClientRpc();
    }

    [ClientRpc]
    private void JClientRpc()
    {
        redFlag.gameObject.SetActive(true);
       
        
    }
}
