using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour {


    public static LobbyCreateUI Instance { get; private set; }


    [SerializeField] private Button createButton;
   
    [SerializeField] private Button publicPrivateButton;
    
    [SerializeField] private Button gameModeButton;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI publicPrivateText;
    [SerializeField] private TextMeshProUGUI maxPlayersText;
    [SerializeField] private TextMeshProUGUI gameModeText;


    private string lobbyName;
    private bool isPrivate;
    private int maxPlayers;
    private LobbyManager.GameMode gameMode;

    private void Awake() {
        Instance = this;

        createButton.onClick.AddListener(() => {
            LobbyManager.Instance.CreateLobby(
              
                lobbyName,
                maxPlayers,
                isPrivate,
                gameMode
            );
            
            Hide();
        });
        publicPrivateButton.onClick.AddListener(() =>
        {
            switch(isPrivate)
            {
                case true:
                    isPrivate= false; 
                    break;
                case false:
                    isPrivate= true;
                    break;
            }
            UpdateText();
        });
        gameModeButton.onClick.AddListener(() => {
            switch (gameMode) {
             
                case LobbyManager.GameMode.PVE:
                    gameMode = LobbyManager.GameMode.Conquest;
                    break;
                case LobbyManager.GameMode.Conquest:
                    gameMode = LobbyManager.GameMode.PVE;
                    break;
                
            }
            //Debug.Log(lobbyName+maxPlayers);
            UpdateText();
        });

        Hide();
    }

    private void UpdateText() {
        
        publicPrivateText.text = isPrivate ? "Private" : "Public";
        
        gameModeText.text = gameMode.ToString();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
    public void OnChangeLobbyName()
    {
        lobbyName = lobbyNameText.text;
    }
    public void OnChangeNumPlayerMax()
    {
        
        string temp = maxPlayersText.text.Trim();
        temp = temp.Substring(0, temp.Length - 1);

        if (int.TryParse(temp, out int tempmax))
        {
            maxPlayers = tempmax;

        }
        else
        {
            maxPlayers = 5;
        }
        
    }
    public void Show() {
        gameObject.SetActive(true);
        
        lobbyName = "MyLobby";
        isPrivate = false;
        maxPlayers = 4;
        gameMode = LobbyManager.GameMode.PVE;

        UpdateText();
    }

}