using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public CardData data;

    Collider2D col;

    public Vector3 StartDragPos;

    [SerializeField] private float CardZWhileDragging;

    private CardDropArea dropArea;

    private void Start()
    {
        dropArea = GameObject.Find("CentralDeck").GetComponent<CardDropArea>();
    }

    public void SetCardData(CardData newData)
    {
        data = newData;
        col = GetComponent<Collider2D>();
        ChangeSprite();
    }

    private void OnMouseDown()
    {
       // DropCard();
    }

    public void DropCard()
    {
        dropArea.DropCard(this);
    }

    private void ChangeSprite()
    {
        string SpriteName = $"{data.Number}_{data.Suit}";
        Sprite CardSprite = Resources.Load<Sprite>($"Cards/{SpriteName}");

        if (CardSprite != null )
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = CardSprite;
            transform.rotation = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);
        }
        else
        {
            Debug.LogWarning($"Card Sprite Not Found: {SpriteName}");
        }

    }
}
