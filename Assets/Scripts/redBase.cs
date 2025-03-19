using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class redBase : NetworkBehaviour
{
    public GameObject blueFlag;
    
    private void Start()
    {
         GameManager.instance.scoreR.OnValueChanged += (a, b) =>  GameManager.instance.scoreBlue.text = b.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Red" && other.GetComponent<PlayerNetwork>().getFlag.Value == true)
        {
            other.gameObject.transform.position = GameManager.instance.spawnRed.position;
            var player = other.gameObject.GetComponent<PlayerNetwork>();
            player.DisableFlagPlayerServerRpc();
            JServerRpc();
            
            
        }
    }

    
    [ServerRpc(RequireOwnership = false)] 
    private void JServerRpc() 
    {
        var g = GameManager.instance.scoreR.Value += 1;
        JClientRpc();
    }

    [ClientRpc]
    private void JClientRpc()
    {
        
        GameManager.instance.scoreRed.text =  GameManager.instance.scoreR.Value.ToString();
        blueFlag.gameObject.SetActive(true);
    }

}
