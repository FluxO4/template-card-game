using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class MainController : MonoBehaviour
{
    public static MainController instance;
    public Transform cameraHolder;
    

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Multiple main controllers, deactivating");
            gameObject.SetActive(false);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < players.Count; i++)
        {
            players[i].index = i;
        }


        cardDeck.PrepareCards();
        cardDeck.ShuffleCards();
        cardDeck.DistributeCards(5);

    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(cameraHolder.localEulerAngles.x) > 0.01f)
        {
            cameraHolder.localEulerAngles = new Vector3(Mathf.LerpAngle(cameraHolder.localEulerAngles.x, 0, 0.05f), 0, 0);
        }
        else
        {
            cameraHolder.localEulerAngles = Vector3.zero;
        }


        if (cardsDistributed && turn == 0)
        {

            // Check if the left mouse button was released
            if (Input.GetMouseButtonUp(0))
            {
                // Create a ray from the mouse cursor on screen in the direction of the camera
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Perform the raycast
                if (Physics.Raycast(ray, out hit))
                {
                    // Output the name of the object that was clicked
                    Debug.Log("Object clicked: " + hit.collider.gameObject.name);

                    if (hit.collider.tag == "Card")
                    {
                        Card cardHit = hit.transform.GetComponent<Card>();

                        if (cardHit.belongsTo == 0)
                        {
                            players[0].SelectCard(cardHit);
                        }
                    }

                    // You can return the object if needed, for example:
                    // return hit.collider.gameObject;
                }
                else
                {
                    Debug.Log("No object hit by raycast.");
                }
            }
        }
    }
    int turn = 0;

    public void startTurnCounter()
    {
        StartCoroutine(turnCounter());
    }

    public void openCallBox()
    {
        callBox.SetActive(true);
    }


    public GameObject callBox;

    public void SelectCall(int call)
    {
        playerCalls[0].text = call.ToString();

        for(int i = 1; i <= 3; i++)
        {
            playerCalls[i].text = Random.Range(1, 5).ToString();
        }

        callBox.SetActive(false);

        startTurnCounter();
    }

    IEnumerator turnCounter()
    {
        turn = 0;

        while (players[3].myCards.Count > 0)
        {
            if(turn == 0)
            {
                while (!playerPlayed)
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
                players[turn].PlayCard(players[turn].myCards[Random.Range(0, players[turn].myCards.Count)]);
            }
            Debug.Log("card played");

            yield return new WaitForSeconds(0.5f);

            turn += 1;

            if(turn >= players.Count)
            {
                turn = 0;

                yield return new WaitForSeconds(1f);
                players[winningPlayer].Score += 1;

                for (int i = 0; i < players.Count; i++)
                {
                    players[i].DiscardPlayedCard(players[winningPlayer].transform.position - players[winningPlayer].transform.forward * 5);
                    

                    playerScores[i].text = players[i].Score.ToString();
                }

            }


            playerPlayed = false;
        }
    }

    int winningPlayer = 0;
    public void ReorderPlayedCards()
    {
        List<int> playedCardValues = new List<int>();

        for(int i = 0; i < players.Count; i++)
        {
            if (players[i].playedCard)
            {
                playedCardValues.Add(players[i].playedCard.value);
            }
            else
            {
                playedCardValues.Add(0);
            }
        }

        var indexedValues = playedCardValues.Select((value, index) => (index, value)).ToList();
        indexedValues.Sort((a, b) => a.value.CompareTo(b.value));
        List<int> ranks = new List<int>(new int[playedCardValues.Count]);

        winningPlayer = indexedValues[3].index;

        for (int i = 0; i < indexedValues.Count; i++)
        {
            ranks[indexedValues[i].index] = i;
        }

        for(int i = 0; i < players.Count; i++)
        {
            playSlots[i].transform.position = new Vector3(playSlots[i].transform.position.x, 0.1f * ranks[i], playSlots[i].transform.position.z);

            if (players[i].playedCard)
            {
                players[i].playedCard.moveTo(playSlots[i].position, playSlots[i].rotation);
            }
        }

    }


    public bool playerPlayed = false;
    public bool cardsDistributed = false;

    //Prefabs
    public GameObject P_cardPrefab;
    public GameObject P_throwCardPrefab;
    

    public List<Sprite> CardImages;

    //Scene Objects
    public List<Player> players;
    public List<Transform> playSlots;
    public List<Text> playerScores;

    public List<Text> playerCalls;

    public Deck cardDeck;




    
}
