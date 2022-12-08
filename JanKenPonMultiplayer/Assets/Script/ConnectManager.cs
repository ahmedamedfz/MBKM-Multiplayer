using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class ConnectManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField usetrnameInput;
    [SerializeField] TMP_Text feedbackText;

    public void ClickConnect(){
        feedbackText.text = "";

        if(usetrnameInput.text.Length < 3){
            feedbackText.text = "Username min 3 Characters";
            return;
        }
        PhotonNetwork.NickName = usetrnameInput.text;
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings();
        feedbackText.text = "Connecting...";
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        feedbackText.text = "Connected to Master";
        SceneManager.LoadScene("Lobby");
    }

    IEnumerator LoadLevelAfterConnectedAndReady()
    {
        while(PhotonNetwork.IsConnectedAndReady == false)
            yield return null;

        SceneManager.LoadScene("Lobby");
    }
}
