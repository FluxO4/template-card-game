using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public GameObject cardsVisual;
    public GameObject indicatorVisual;
 

    public List<Card> cards = new List<Card>();

    public void PrepareCards()
    {
        Debug.Log("Preparing cards");
        for (int i = 0; i < 52; i++)
        {
            Card newCard = Instantiate(MainController.instance.P_cardPrefab, transform).GetComponent<Card>();
            newCard.faceMat.material.SetTexture("_MainTex", MainController.instance.CardImages[i].texture);
            cards.Add(newCard);
            newCard.nameValue = i;

        }
    }

    public void ShuffleCards(int numSwaps = 100)
    {
        Debug.Log("Shuffling cards");
        if (cards.Count > 0)
        {
            for(int i = 0; i < numSwaps; i++)
            {
                int card1 = Random.Range(0, cards.Count);
                int card2 = Random.Range(0, cards.Count);
                Card temp = cards[card1];
                cards[card1] = cards[card2];
                cards[card2] = temp;
            }
        }
        Debug.Log("Card shuffling complete");
    }

    public void DistributeCards(int numToEachPlayer)
    { // negative for full deck distribution
        StartCoroutine(DistributeCardsCoroutine(numToEachPlayer));
    }

    IEnumerator DistributeCardsCoroutine(int numToEachPlayer)
    {
        Debug.Log("Starting card distribution");
        int playerToSendCardTo = 0;
        int sentToEachPlayer = 0;

        while (cards.Count > 0 || sentToEachPlayer == numToEachPlayer)
        {
            Card nextCard = cards[0];
            nextCard.transform.position += Vector3.up * 3;
            cards.RemoveAt(0);
            if (cards.Count == 0)
            {
                cardsVisual.SetActive(false);
            }

            nextCard.belongsTo = playerToSendCardTo;
            SendCard(nextCard, MainController.instance.players[playerToSendCardTo]);
            playerToSendCardTo += 1;

            if (playerToSendCardTo >= MainController.instance.players.Count)
            {
                playerToSendCardTo = 0;
                sentToEachPlayer += 1;
            }

            Debug.Log("Sent card");
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("Card distribution complete");

        MainController.instance.cardsDistributed = true;

        MainController.instance.openCallBox();
    }

    public void SendCard(Card card, Player player)
    {
        card.activateVisual();
        
        player.AddCard(card);
    }
}
