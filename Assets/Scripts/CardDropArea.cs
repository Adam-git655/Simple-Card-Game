using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardDropArea : MonoBehaviour
{

    public DeckManager deckManager;

    private float cardZ = 0f;
    private float cardY;   
    private readonly float cardZStepIncrement = -0.01f;
    private readonly float cardYStepIncrement = 0.01f;

    public List<CardData> DiscardPile = new();

    private Transform LastCardDeck;

    public Text WinText;

    public AudioSource CardSound;
    public AudioSource TrickWonSound;
    public AudioSource GameWonSound;
    private void Start()
    {
        cardY = transform.position.y;
        WinText.enabled = false;
        //PlayAgainButton.gameObject.SetActive(false);
    }

    public void DropCard(CardDisplay card)
    {
        CardSound.Play();

        card.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        LastCardDeck = card.transform.parent;

        deckManager.Player1Deck.Remove(card.transform);
        deckManager.Player2Deck.Remove(card.transform);

        StartCoroutine(Lerp.MoveObjectFromPointAToPointBOverTime(card.transform, card.transform.position, new Vector3(transform.position.x, cardY, cardZ), 0.2f));
        card.transform.rotation = Quaternion.identity;

        card.transform.SetParent(transform);
        
        cardZ += cardZStepIncrement;
        cardY += cardYStepIncrement;

        DiscardPile.Add(card.data);

        if (deckManager.Player1Deck.Count < 1)
        {
            WinText.enabled = true;
            WinText.text = "PLAYER 2 WINS!";
            GameWonSound.Play();
            //PlayAgainButton.gameObject.SetActive(true);
        }
        else if (deckManager.Player2Deck.Count < 1)
        {
            WinText.enabled = true;
            WinText.text = "PLAYER 1 WINS!";
            GameWonSound.Play();
            //PlayAgainButton.gameObject.SetActive(true);
        }

        TurnManager.Instance.EndTurn();

        if (DiscardPile.Count > 1)
        {
            CardData previousCard = DiscardPile[^2];

            if (previousCard.Number == card.data.Number)
            {
                StartCoroutine(TrickWon(card));
            }
        }
    }

    private IEnumerator TrickWon(CardDisplay card)
    {
        List<Transform> otherDeckList;

        if (LastCardDeck.gameObject.name == "Player2Deck")
            otherDeckList = deckManager.Player1Deck;
        else
            otherDeckList = deckManager.Player2Deck;

        otherDeckList[^1].GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(0.8f);

        otherDeckList[^1].GetComponent<BoxCollider2D>().enabled = true;

        TrickWonSound.Play();

        List<Transform> wonCards = new List<Transform>();
        foreach (Transform wonCard in transform)
        {
            wonCards.Add(wonCard);
        }

        List<Transform> targetDeckList;

        if (LastCardDeck.gameObject.name == "Player2Deck")
            targetDeckList = deckManager.Player2Deck;
        else
            targetDeckList = deckManager.Player1Deck;

        float startY = LastCardDeck.position.y;
        float startZ = LastCardDeck.position.z;

        if (targetDeckList.Count > 0)
        {
            startY = targetDeckList[^1].position.y;
            startZ = targetDeckList[^1].position.z;
        }
        
        foreach (Transform wonCard in wonCards)
        {
            wonCard.transform.rotation = Quaternion.Euler(0, 180, 0);

            startY += cardYStepIncrement;
            startZ += cardZStepIncrement;

            Vector3 targetPos = new(LastCardDeck.position.x, startY, startZ);
            StartCoroutine(Lerp.MoveObjectFromPointAToPointBOverTime(wonCard, wonCard.transform.position, targetPos, 0.3f));

            wonCard.SetParent(LastCardDeck);
            targetDeckList.Add(wonCard);
        }

        cardY = transform.position.y;
        cardZ = 0f;
        DiscardPile.Clear();
    }

    public void OnPlayAgainButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
