using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour {


    [SerializeField] private Button authenticateButton;
    [SerializeField] private TextMeshProUGUI Warning;


    private void Awake() {
        authenticateButton.onClick.AddListener(() => {
            if (EditPlayerName.Instance.GetPlayerName()!="")
            {
                LobbyManager.Instance.Authenticate(EditPlayerName.Instance.GetPlayerName());
                Hide();
            }
            else
            {
                Warning.text = "Veuillez D'abord choisir un pseudo !";
            }
        });
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}