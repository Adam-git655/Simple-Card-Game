using UnityEngine;

public class TouchManager : MonoBehaviour
{

    public DeckManager deckManager;

    private bool canDistributeCardsAtStartOfGame = true;

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (canDistributeCardsAtStartOfGame)
            {
                canDistributeCardsAtStartOfGame = false;
                StartCoroutine(deckManager.DistributeCards());
            }


            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Card"))
            {
                CardDisplay card = hit.collider.gameObject.GetComponent<CardDisplay>();
                if (card != null)
                {
                    Debug.Log("Droppped touch");
                    card.DropCard();
                }
            }

        }
    }
}
