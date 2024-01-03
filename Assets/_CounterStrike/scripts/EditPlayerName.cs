using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditPlayerName : MonoBehaviour {


    public static EditPlayerName Instance { get; private set; }


    [SerializeField] private TextMeshProUGUI playerNameText;


    private string playerName = "example";


    private void Awake() {
        Instance = this;
      
        playerName = playerNameText.text;
    }

    
   
    public void ChangedName()
    {
        playerName = playerNameText.text;
        LobbyManager.Instance.UpdatePlayerName(playerName);
    }
    public string GetPlayerName() {
         Debug.Log(playerName);

        //return playerName;
        playerName = playerName.Substring(0, playerName.Length - 1);
        return playerName;
    }


}