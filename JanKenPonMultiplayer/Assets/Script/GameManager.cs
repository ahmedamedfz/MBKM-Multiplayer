using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using TMPro;
using Photon.Pun;
using OnlineGame;


namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public GameObject onPlayerPrefab;
        public Player P1;
        public Player P2;
        public GameState State = GameState.NetPlayersIntialization;
        public GameObject gameOverPanel;
        public TMP_Text winnerText;
        private Player damagedPlayer;
        private Player winner;
        public enum GameState 
        {
            NetPlayersIntialization,
            ChooseAttack,
            Attack,
            Damages,
            Draw,
            GameOver,
        }

        private void Start()
        {
            gameOverPanel.SetActive(false);
            PhotonNetwork.Instantiate(onPlayerPrefab.name,Vector3.zero,Quaternion.identity);
        }
        private void Update()
        {
            switch(State)
            {
                case GameState.NetPlayersIntialization:
                    if (OnlinePlayer.onPlayers.Count == 2)
                    {
                        foreach (var netPlayer in OnlinePlayer.onPlayers)
                        {
                            if(netPlayer.photonView.IsMine){
                                netPlayer.Set(P1);
                            }
                            else
                            {
                                netPlayer.Set(P2);
                            }
                        }
                        State = GameState.ChooseAttack;
                    }
                    break;
                case GameState.ChooseAttack:
                    if (P1.AttackValue != null && P2.AttackValue!= null)
                    {
                        P1.AnimateAttack();
                        P2.AnimateAttack();
                        P1.IsClickable(false);
                        P2.IsClickable(false);
                        State = GameState.Attack;
                    }
                    break;
                case GameState.Attack:
                    if (P1.IsAnimating() == false && P2.IsAnimating() == false)
                    {
                        damagedPlayer = GetDamagedPlayer();
                        if(damagedPlayer != null)
                        {
                            damagedPlayer.AnimateDamage();
                            State = GameState.Damages;
                        }
                        else
                        {
                            P1.AnimateDraw();
                            P2.AnimateDraw();
                            State = GameState.Draw;
                        }
                    }
                    break;
                case GameState.Damages:
                    if(P1.IsAnimating() == false && P2.IsAnimating() == false)
                    {
                        if (damagedPlayer == P1)
                        {
                            P1.ChangeHealth(-10);
                            P2.ChangeHealth(10);
                        }
                        else
                        {
                            P2.ChangeHealth(-10);
                            P1.ChangeHealth(10);
                        }

                        var winner = GetWinner();

                        if(winner == null)
                        {
                            ResetPlayers();
                            P1.IsClickable(true);
                            P2.IsClickable(true);
                            State = GameState.ChooseAttack;
                        }
                        else
                        {
                            gameOverPanel.SetActive(true);
                            winnerText.text = winner == P1 ? $"{P1.NickName.text} is The Winner" : $"{P2.NickName.text} is The Winner";
                            ResetPlayers();
                            State = GameState.GameOver;
                        }
                    }
                    break;
                case GameState.Draw:
                    if(P1.IsAnimating() == false && P2.IsAnimating() == false)
                    {
                        ResetPlayers();
                        P1.IsClickable(true);
                        P2.IsClickable(true);
                        State = GameState.ChooseAttack;
                    }
                    break;
            }
        }
        private void ResetPlayers()
        {
            damagedPlayer = null;
            P1.Reset();
            P2.Reset();
        }

        private Player GetDamagedPlayer()
        {
            Attack? PlayerAtk1 = P1.AttackValue;
            Attack? PlayerAtk2 = P2.AttackValue;

            if(PlayerAtk1 == Attack.Rock && PlayerAtk2 == Attack.Paper)
            {
                return P1;
            }
            else if(PlayerAtk1 == Attack.Rock && PlayerAtk2 == Attack.Scissor)
            {
                return P2;
            }
            else if(PlayerAtk1 == Attack.Paper && PlayerAtk2 == Attack.Rock)
            {
                return P2;
            }
            else if(PlayerAtk1 == Attack.Paper && PlayerAtk2 == Attack.Scissor)
            {
                return P1;
            }
            else if(PlayerAtk1 == Attack.Scissor && PlayerAtk2 == Attack.Rock)
            {
                return P1;
            }
            else if(PlayerAtk1 == Attack.Scissor && PlayerAtk2 == Attack.Paper)
            {
                return P2;
            }
            return null;
        }

        private Player GetWinner()
        {
            if(P1.Health == 0)
            {
                return P2;
            }
            else if (P2.Health == 0)
            {
                return P1;
            }
            else
            {
                return null;
            }
        }
    }
}
