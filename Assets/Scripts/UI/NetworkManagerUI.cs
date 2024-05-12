using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private short maxPlayers = 2;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject hostPanel;
    [SerializeField] private GameObject clientPanel;
    [SerializeField] private GameObject clientJoinedPanel;
    [SerializeField] private TextMeshProUGUI roomCodeText;
    [SerializeField] private TMP_InputField joinCodeInputField;
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button startGameButton;
    [SerializeField] private TMP_Text p2StatusText;


    public event Action OnClientJoined;

    private string joinCode;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed In" + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        createRoomButton.interactable = true;
        createRoomButton.GetComponentInChildren<TMP_Text>().text = "Create";

        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
    }

    private void HandleServerStarted()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
    }

    private void HandleClientConnected(ulong clientId)
    {
        // Check if the connected client is not the server
        if (clientId != NetworkManager.Singleton.LocalClientId)
        {
            startGameButton.interactable = true;
            p2StatusText.text = "P2 joined!";
            p2StatusText.color = new Color32(135, 16, 26, 255);
        }
    }

    private void HandleClientDisconnected(ulong clientId)
    {
        // Check if the disconnected client is the server
        if (clientId == NetworkManager.ServerClientId)
        {
            // The host has disconnected
            clientJoinedPanel.SetActive(false);
            startPanel.SetActive(true);
        }
        else if (clientId != NetworkManager.Singleton.LocalClientId)
        {
            // A client other than the host has disconnected
            //startGameButton.interactable = false;
            p2StatusText.text = "Waiting for P2...";
            p2StatusText.color = new Color32(89, 89, 89, 255);
        }
        else if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            // The local client has disconnected
            clientJoinedPanel.SetActive(false);
            startPanel.SetActive(true);
        }
    }

    public async void CreateRoom()
    {
        try
        {
            createRoomButton.interactable = false;
            createRoomButton.GetComponentInChildren<TMP_Text>().text = "Loading";
            Debug.Log("Host - Creating an allocation.");
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers - 1);
            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            roomCodeText.text = joinCode;  // Display the code in the host panel
            Debug.Log(joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();

            startPanel.SetActive(false);
            hostPanel.SetActive(true);  // Enable host panel to display the room code
            //startGameButton.interactable = false;
        }
        catch (RelayServiceException e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void EnableClientPanel()
    {
        startPanel.SetActive(false);
        clientPanel.SetActive(true);  // Show the client panel for joining
    }

    public void OnBackFromHost()
    {
        NetworkManager.Singleton.Shutdown(); // Stop the network session

        hostPanel.SetActive(false);
        startPanel.SetActive(true); // Show the start panel again

        createRoomButton.interactable = true;
        createRoomButton.GetComponentInChildren<TMP_Text>().text = "Create Room";
    }

    public void OnBackFromClient()
    {
        joinCodeInputField.text = "";
        clientPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void OnBackFromClientJoined()
    {
        joinCodeInputField.text = "";
        clientJoinedPanel.SetActive(false);
        startPanel.SetActive(true);
        NetworkManager.Singleton.Shutdown(); // Stop the network client
    }

    public async void JoinRoom()
    {
        try
        {
            string code = joinCodeInputField.text;
            Debug.Log("Joining Relay with " + code);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(code);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();

            clientPanel.SetActive(false);
            clientJoinedPanel.SetActive(true);

            OnClientJoined?.Invoke();
        }
        catch (RelayServiceException e)
        {
            Debug.LogError(e.Message);
        }
    }


    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;

            if (NetworkManager.Singleton.IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            }
        }
    }
}
