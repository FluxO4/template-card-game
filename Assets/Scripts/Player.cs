using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public bool mainPlayer = false;
    public int index = 0;
    void Start()
    {
        if (mainPlayer)
        {
            nextCardSlot.localEulerAngles = new Vector3(0, 0, 160);
        }
        else
        {
            nextCardSlot.localEulerAngles = new Vector3(0, 0, -20);
        }
    }


    bool moving = false;

    Vector3 targetPosition = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if((cardHolderParent.localPosition - targetPosition).sqrMagnitude > 0.01)
            {
                cardHolderParent.localPosition = Vector3.Lerp(cardHolderParent.localPosition, targetPosition, 0.1f);
            }
            else
            {
                moving = false;

            }
        }
    }

    public float cardSpacing = 0.2f;

    public float cardSelectionMoveAmount = 0.3f;

    public Transform cardHolderParent;
    public Transform nextCardSlot;

    public List<Card> myCards = new List<Card>();

    public Card selectedCard;
    public Card playedCard;

    public int Score = 0;


    public void AddCard(Card cardToAdd)
    {
        myCards.Add(cardToAdd);
        cardToAdd.transform.SetParent(cardHolderParent);
        ResetPositions();
    }

    public void ResetPositions()
    {
        

        for (int i = 0; i < myCards.Count; i++)
        {
            nextCardSlot.localPosition = new Vector3(i * cardSpacing, 0, 0);
            myCards[i].moveToLocal(nextCardSlot.localPosition, nextCardSlot.localRotation);
        }

        targetPosition = new Vector3(-myCards.Count * cardSpacing * 0.5f, 0, 0);
        moving = true;
    }

    public void SelectCard(Card cardToSelect)
    {
        if(selectedCard == cardToSelect)
        {
            PlaySelectedCard();
            return;
        }

        ResetPositions();

        cardToSelect.moveToLocal(cardToSelect.transform.localPosition + cardToSelect.transform.forward * cardSelectionMoveAmount, cardToSelect.transform.localRotation);
        selectedCard = cardToSelect;
    }

    public void PlaySelectedCard()
    {
        PlayCard(selectedCard);
        selectedCard = null;
    }

    public void PlayCard(Card cardToPlay)
    {
        cardToPlay.moveTo(MainController.instance.playSlots[index].position, MainController.instance.playSlots[index].rotation);
        playedCard = cardToPlay;

        myCards.Remove(cardToPlay);

        MainController.instance.ReorderPlayedCards();

        ResetPositions();

        MainController.instance.playerPlayed = true;
    }

    public void DiscardPlayedCard(Vector3 destination)
    {
        MainController.instance.cardDeck.cards.Add(playedCard);
        //MainController.instance.cardDeck.cardsVisual.SetActive(true);

        playedCard.moveTo(destination, Quaternion.identity);
        
        playedCard.deactivateVisualAfter();
        playedCard = null;
    }

}
