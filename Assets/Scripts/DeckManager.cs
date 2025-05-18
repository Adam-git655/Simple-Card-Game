using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<CardData> FullDeck = new();
    public List<Transform> Player1Deck = new(); 
    public List<Transform> Player2Deck = new();

    public GameObject CardPrefab;
    public Transform Player1DeckTransform;
    public Transform Player2DeckTransform;
    public Transform CentralDeckTransform;

    public AudioSource CardSound;

    private float StartCardY = 0.0f;
    private readonly float CardStepYIncrement = 0.01f;

    private float StartCardZ = 0f;
    private readonly float CardStepZIncrement = -0.01f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateFullDeck();
        ShuffleCards(FullDeck);
        TurnManager.Instance.OnTurnChanged += HandleTurnChange;
    }

    void HandleTurnChange(TurnManager.PlayerTurns newTurn)
    {
        if (TurnManager.Instance.IsPlayerTurn(TurnManager.PlayerTurns.Player1Turn))
        {
            if (Player1Deck.Count > 0)
                Player1Deck[^1].GetComponent<BoxCollider2D>().enabled = true;

        }
        else
        {
            if (Player2Deck.Count > 0)
                Player2Deck[^1].GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    void GenerateFullDeck()
    {
        string[] suits = { "Hearts", "Spades", "Clubs", "Diamonds" };

        for (int i = 1; i < 14; i++)
        {
            foreach (var suit in suits)
            { 
                FullDeck.Add(new CardData { Number = i, Suit = suit });

            }
        }

    }

    void ShuffleCards(List<CardData> deck)
    {
        for (int i = 0; i < deck.Count ;i++)
        {
            CardData temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
        DisplayFullDeck();
    }

    void DisplayFullDeck()
    {
        for (int i = 0; i < FullDeck.Count;i++)
        { 
            var CardInstance = Instantiate(CardPrefab);
            CardInstance.GetComponent<CardDisplay>().SetCardData(FullDeck[i]);
            CardInstance.GetComponent<BoxCollider2D>().enabled = false;
            CardInstance.transform.SetParent(CentralDeckTransform);
            CardInstance.transform.position = CentralDeckTransform.position;
            CardInstance.transform.position += new Vector3(0.0f, StartCardY, StartCardZ);
            StartCardY += CardStepYIncrement;
            StartCardZ += CardStepZIncrement;
        }
    }

    public IEnumerator DistributeCards()
    {
        StartCardY = 0.0f;
        StartCardZ = 0.0f;

        List<Transform> cards = new List<Transform>();

        for (int i = CentralDeckTransform.childCount - 1; i >= 0; i--)
        {
            cards.Add(CentralDeckTransform.GetChild(i));
        }


        for (int i = 0; i < cards.Count; i++)
        {
            Transform card = cards[i];
            Vector3 targetPosition;

            if (i % 2 == 0)
            {
                card.SetParent(Player2DeckTransform);
                targetPosition = new Vector3(Player2DeckTransform.position.x, StartCardY, StartCardZ);
                Player2Deck.Add(card);
            }
            else
            {
                card.SetParent(Player1DeckTransform);
                targetPosition = new Vector3(Player1DeckTransform.position.x, StartCardY, StartCardZ);
                Player1Deck.Add(card);
            }

            StartCardY += CardStepYIncrement;
            StartCardZ += CardStepZIncrement;
            yield return StartCoroutine(Lerp.MoveObjectFromPointAToPointBOverTime(card, card.position, targetPosition, 0.05f));
            CardSound.Play();
        }
        Player1Deck[^1].GetComponent<BoxCollider2D>().enabled = true;
    }

}
