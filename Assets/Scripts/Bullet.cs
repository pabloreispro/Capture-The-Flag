using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public GameObject redFlag, blueFlag;
    Vector3 direction;
    

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction*speed*Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "scenario")
        {
            GetComponent<NetworkObject>().DontDestroyWithOwner = true;
            GetComponent<NetworkObject>().Despawn();
        }
        if(other.tag == "Red")
        {
            other.gameObject.transform.position = GameManager.instance.spawnRed.position;
            if(other.GetComponent<PlayerNetwork>().getFlag.Value == true)
            {
                var player = other.gameObject.GetComponent<PlayerNetwork>();
                player.DisableFlagPlayerServerRpc();
                RedServerRpc();
            }
            GetComponent<NetworkObject>().DontDestroyWithOwner = true;
            GetComponent<NetworkObject>().Despawn();
        }
        if(other.tag == "Blue")
        {
            other.gameObject.transform.position = GameManager.instance.spawnBlue.position;
            
            if(other.GetComponent<PlayerNetwork>().getFlag.Value == true)
            {
                var player = other.gameObject.GetComponent<PlayerNetwork>();
                player.DisableFlagPlayerServerRpc();
                BlueServerRpc();
            }
            GetComponent<NetworkObject>().DontDestroyWithOwner = true;
            GetComponent<NetworkObject>().Despawn();
        }
        
    }

    [ServerRpc]
    private void BlueServerRpc()
    {
        BlueClientRpc();
    }
    [ServerRpc]
    private void RedServerRpc()
    {
        RedClientRpc();
    }

    [ClientRpc]
    private void BlueClientRpc()
    {
        
        blueFlag.gameObject.SetActive(true);
    }
    [ClientRpc]
    private void RedClientRpc()
    {
        
        redFlag.gameObject.SetActive(true);
    }
}
