using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Button hostButton;
    [SerializeField] Button joinButton;

    void Start()
    {
        hostButton.onClick.AddListener(() =>
        {
            Debug.Log("a");
            NetworkManager.Singleton.StartHost();
        });
        joinButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}
