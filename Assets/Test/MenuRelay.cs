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

public class MenuRelay : MonoBehaviour
{
    private bool signIn = false;
    [SerializeField] Button hostButton;
    [SerializeField] Button joinButton;
    [SerializeField] TMP_InputField textInput;

    private void Start()
    {
        hostButton.onClick.AddListener(() =>
        {
            CreateRelay();
        });
        joinButton.onClick.AddListener(() =>
        {
            if (textInput.text == null)
                return;
            JoinRelay(textInput.text);
        });
        if (UnityServices.State != ServicesInitializationState.Initialized) StartRelay();
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

    private async void JoinRelay(string joinCode)
    {
        try
        {
            PlayerPrefs.SetString("Code", joinCode);

            Debug.Log("Try to join relay with code " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
}
