using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditPlayerName : MonoBehaviour {


    public static EditPlayerName Instance { get; private set; }
    bool DoOnce = true;

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
         Debug.Log(playerName.Length);
        if (DoOnce)
        {
            playerName = playerName.Substring(0, playerName.Length - 1);
            DoOnce = false;
        }
        //return playerName;
       
        return playerName;
    }


}