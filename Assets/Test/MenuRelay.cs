using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine.UI;
using UnityEngine.XR;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Windows;

public class MenuRelay : MonoBehaviour
{
    private bool signIn = false;
    
    [SerializeField]
    private Button buttonServer, buttonHost, buttonClient, buttonRelay, buttonLobby, buttonLobbyList, refreshLobby;
    [SerializeField]
    private TextMeshProUGUI textInfo, codeLobby;
    [SerializeField]
    private TMP_InputField input, inputLobby, inputGameMode, inputNewName, inputLobbyName;
    private Lobby hostLobby, joinedLobby;
    private float timerBeatLobbyMax = 15f;
    private float timerUpdateLobbyMax = 1.1f;
    private float timerBeatLobby, timerUpdateLobby;
    private string playerName;
    [SerializeField] TMP_InputField textInput;

    private void Start()
    {
        //hostButton.onClick.AddListener(() =>
        //{
        //    CreateRelay();
        //});
        //joinButton.onClick.AddListener(() =>
        //{
        //    if (textInput.text == null)
        //        return;
        //    JoinRelay(textInput.text);
        //});
        //if (UnityServices.State != ServicesInitializationState.Initialized) StartRelay();
    }
    private void Update()
    {
        beatHostLobby();
        updateLobby();
    }

    private async void StartRelay()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
            signIn = true;
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);
            PlayerPrefs.SetString("Code", joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);


            NetworkManager.Singleton.StartHost();

        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

  

    // lobby manager
    private void RefreshLobby()
    {
        if (hostLobby == joinedLobby)
        {
            codeLobby.text = "Code of Lobby " + hostLobby.Data["LobbyName"].Value + " : " + hostLobby.LobbyCode;
        }
        else
        {
            codeLobby.text = "Vous avez rejoint le lobby : " + joinedLobby.Data["LobbyName"].Value;
        }
        textInfo.text = "";
        int n = 1;
        if (joinedLobby != null)
        {
            foreach (Unity.Services.Lobbies.Models.Player player in joinedLobby.Players)
            {
                textInfo.text += "Player " + n + " : " + player.Data["PlayerName"].Value + "\n";
                n++;
            }
            textInfo.text += "GameMode : " + joinedLobby.Data["GameMode"].Value;
        }
    }
    private async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joing Relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
            );

            NetworkManager.Singleton.StartClient();
            textInfo.text = "Vous avez bien rejoint la salle";
            buttonRelay.gameObject.SetActive(false);
            input.gameObject.SetActive(false);
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
    private Unity.Services.Lobbies.Models.Player GetPlayer()
    {
        return new Unity.Services.Lobbies.Models.Player
        {
            Data = new Dictionary<string, PlayerDataObject> {
                { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
            }
        };
    }
    private async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayers = 4;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, "DeathMatch")},
                    {"LobbyName", new DataObject(DataObject.VisibilityOptions.Public, lobbyName)}
                }
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);
            hostLobby = lobby;
            joinedLobby = lobby;
            Debug.Log("Created lobby : " + lobby.Name + ", Players Max : " + lobby.MaxPlayers + ", Code : " + lobby.LobbyCode + " Mode : " + lobby.Data["GameMode"].Value);
            RefreshLobby();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter> { new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT) },
                Order = new List<QueryOrder> { new QueryOrder(false, QueryOrder.FieldOptions.Created) }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            Debug.Log("Lobbies found : " + queryResponse.Results.Count);

            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " for " + lobby.MaxPlayers + " players max and " + lobby.AvailableSlots + " more slots availables");
            }
        }

        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private async void beatHostLobby()
    {
        if (hostLobby != null)
        {
            timerBeatLobby -= Time.deltaTime;
            if (timerBeatLobby <= 0)
            {
                timerBeatLobby = timerBeatLobbyMax;
                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }
    private async void updateLobby()
    {
        if (joinedLobby != null)
        {
            timerUpdateLobby -= Time.deltaTime;
            if (timerUpdateLobby <= 0)
            {
                timerUpdateLobby = timerUpdateLobbyMax;
                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;
            }
        }
    }
    private async void JoinLobby(string code)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions { Player = GetPlayer() };
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(code, joinLobbyByCodeOptions);
            joinedLobby = lobby;
            RefreshLobby();
        }

        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private void printPlayers(Lobby lobby)
    {
        foreach (Unity.Services.Lobbies.Models.Player player in lobby.Players)
        {
            textInfo.text += player.Data["PlayerName"].Value;
        }
    }
    public void changeGameMode()
    {
        UpdateGameMode(inputGameMode.text);
    }
    private async void UpdateGameMode(string gameMode)
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject> {
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode) }
                }
            });
            joinedLobby = hostLobby;
            RefreshLobby();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private async void UpdatePlayerName(string newName)
    {
        try
        {
            playerName = newName;
            RefreshLobby();
            await Lobbies.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject>
                {
                    {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, newName) }
                }
            });
            joinedLobby = hostLobby;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
