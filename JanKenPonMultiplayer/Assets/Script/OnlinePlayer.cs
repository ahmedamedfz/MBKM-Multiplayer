using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Game;
namespace OnlineGame
{
public class OnlinePlayer : MonoBehaviourPun
    {
        public static List<OnlinePlayer> onPlayers = new List<OnlinePlayer>(2);

    private Card[] cards;

    public void Set(Player player){
        player.NickName.text = photonView.Owner.NickName;
        cards = player.GetComponentsInChildren<Card>();
        foreach (var card in cards)
        {
            var button = card.GetComponent<Button>();
            button.onClick.AddListener(()=> RemoteClickButton(card.AttackValue));
        
            if(photonView.IsMine == false)
                button.interactable = false;
        }
    }

    private void OnDestroy() {
        foreach (var card in cards)
        {
            var button = card.GetComponent<Button>();
            button.onClick.RemoveListener(()=>RemoteClickButton(card.AttackValue));
        }
    }

    private void RemoteClickButton(Attack value)
    {
        if(photonView.IsMine)
            photonView.RPC("RemoteClickButtonRPC", RpcTarget.Others,(int)value);
    }

    [PunRPC]

    private void RemoteClickButtonRPC(int value)
    {
        foreach (var card in cards)
        {
            if(card.AttackValue == (Attack)value){
                var button = card.GetComponent<Button>();
                button.onClick.Invoke();
                break;
            }
        }
    }

    private void OnEnable() {
        onPlayers.Add(this);
    }

    private void OnDisable() {
        onPlayers.Remove(this);
    }
    }
}