using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour
{
    [Header("Poperty")]
    [SerializeField] private TMP_Text CardName;
    [SerializeField] private TMP_Text CardNumber;
    [SerializeField] private TMP_Text CardSuit;
    [SerializeField] private SpriteRenderer imageSR;
    [SerializeField] private GameObject wrapper;
    private bool isSelected = false;    
    public CardInstance Card { get; private set; }

    public void Setup(CardInstance card)
    {
        Card = card;
        CardName.text = Card.Data.CardName;
        CardNumber.text = Card.Number.ToString();
        CardSuit.text = Card.Suit.ToSymbol();
        imageSR.sprite = Card.Data.Image;
    }
    public void ForceUnselect()
    {
        isSelected = false;
        wrapper.SetActive(true);
    }

    void OnMouseDown()
    {
        if (isSelected)
        {
            CardViewHoveSystem.Instance.Hide(transform.position,this);
            wrapper.SetActive(true);
            isSelected = false;
        }
        else
        {
            wrapper.SetActive(false);
            Vector3 pos = new(transform.position.x, -5f, 0f);
            CardViewHoveSystem.Instance.Show(Card, transform.position, pos, this);
            isSelected = true;
        }
    }
    

}
