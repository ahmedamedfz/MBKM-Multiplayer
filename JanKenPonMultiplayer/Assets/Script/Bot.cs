using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
public class Bot : MonoBehaviour
    {
        public Player player;
        public GameManager gameManager;
        public float ChoosingInterval;
        private float timer = 0;
        int lastSelected = 0;
        Card[] cards;
        private void Start()
        {
            cards = GetComponentsInChildren<Card>();
        }
        void Update()
        {
            if(gameManager.State != GameManager.GameState.ChooseAttack){
                timer = 0; 
                return;
            }    

            if(timer < ChoosingInterval){
                timer += Time.deltaTime;
                return;
            }
            timer = 0;
            ChooseAttack();
        }
        public void ChooseAttack(){
            var random = Random.Range(1,cards.Length);
            var selection = (lastSelected + random)% cards.Length;
            player.SetChosenCard(cards[selection]);
            lastSelected = selection;
        }
    }
}