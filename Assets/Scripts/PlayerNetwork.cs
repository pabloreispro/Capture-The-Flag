using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    Rigidbody rb;
    public float speed,rotationSpeed;
    //public bool getFlag;
    public GameObject flag;
    public Material r,b;
    public Transform spawnPrefab,spawnPrefab2;
    public GameObject prefab,prefab2;

    public static PlayerNetwork instance;

    
    public NetworkVariable<bool> getFlag = new NetworkVariable<bool>(false);

    
    [ServerRpc(RequireOwnership = false)]
    public void FlagPlayerServerRpc()
    {
        FlagPlayerClientRpc();
        getFlag.Value = true;
    }

    
    [ClientRpc]
    private void FlagPlayerClientRpc()
    {
        flag.SetActive(true);
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void DisableFlagPlayerServerRpc()
    {
        DisableFlagPlayerClientRpc();
        getFlag.Value = false;
    }

    
    [ClientRpc]
    private void DisableFlagPlayerClientRpc()
    {
        flag.SetActive(false);
        
    }
    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        
    }
    
    void FixedUpdate()
    {
        
        if(IsOwner)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                
                FazSpawn_ServerRpc(transform.forward);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                
                FazSpawn2_ServerRpc();
            }
            MovementPlayer();

            
        }
        
    }

    public void MovementPlayer()
    {
        Vector3 inputMove = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
       

       if (inputMove != Vector3.zero)
        {
            // Calcular a rotação alvo
            Quaternion targetRotation = Quaternion.LookRotation(inputMove, Vector3.up);

            // Rotação suave do jogador
            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        
        // Mover o jogador
        rb.MovePosition(transform.position + inputMove * speed * Time.fixedDeltaTime);
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.red);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if(GameManager.instance.redTeam.Count != GameManager.instance.blueTeam.Count && !GameManager.instance.blueTeam.Contains(OwnerClientId))
        {
            GetComponent<Renderer>().material = b;
            flag.GetComponent<Renderer>().material = r;
            GameManager.instance.blueTeam.Add(OwnerClientId);
            transform.position = GameManager.instance.spawnBlue.position;
            gameObject.tag = "Blue";
            
        }
        else if (!GameManager.instance.redTeam.Contains(OwnerClientId))
        {
            GetComponent<Renderer>().material = r;
            flag.GetComponent<Renderer>().material = b;
            GameManager.instance.redTeam.Add(OwnerClientId);
            transform.position = GameManager.instance.spawnRed.position;
            gameObject.tag = "Red";
        }
        
    }


    [ServerRpc] 
    public void FazSpawn_ServerRpc(Vector3 direction)
    {
        var instance = Instantiate(prefab, spawnPrefab.position, Quaternion.identity);
        var bulletScript = instance.GetComponent<Bullet>();
        bulletScript.SetDirection(direction);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn(); 
        
    }
    [ServerRpc]
    public void FazSpawn2_ServerRpc()
    {
        FazSpawn2_ClientRpc();
    }
    [ClientRpc]
    public void FazSpawn2_ClientRpc()
    {
        var instance = Instantiate(prefab2, spawnPrefab2.position, Quaternion.identity);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
    }
    private void OnTriggerEnter(Collider other)
    {
        

        
    }
    
}
