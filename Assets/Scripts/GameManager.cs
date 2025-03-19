using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    
    [SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] public TMP_InputField inputField;


    public List<ulong> redTeam,blueTeam;

    public TextMeshProUGUI scoreRed,scoreBlue;
    
    public Transform spawnRed,spawnBlue;

    
    public NetworkVariable<int> scoreR = new NetworkVariable<int>(0);

    public NetworkVariable<int> scoreB = new NetworkVariable<int>(0);

    private void Start()
    {
        Setup();
        SubscribeToEvents();
        
    }

    

    private void Setup()
    {
        instance = this;
        hostButton.interactable = true;
        serverButton.interactable = true;
        clientButton.interactable = true;
    }

    private void SubscribeToEvents()
    {
        serverButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });

        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });

        clientButton.onClick.AddListener(() =>
        {
            TryConnectClient();
        });

        
    }

    private void TryConnectClient()
    {
        string ipAddress = inputField.text;
        if (ipAddress == null || ipAddress.Length == 0)
        {
            ipAddress = "127.0.0.1";
        }
        UnityTransport transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        transport.ConnectionData.Address = ipAddress;
        //transport.ConnectionData.Port = ushort.Parse("7777");
        NetworkManager.Singleton.StartClient();
    }

    
    
}
