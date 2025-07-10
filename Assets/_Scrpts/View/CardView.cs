using System.Security.Cryptography;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public abstract class CardView : MonoBehaviour
{
    public abstract GameObject wrapper { get; set; }
    public static CardView CurrentlySelectedCard { get; private set; } = null;
    private static bool isOneCardSelected = false;
    public CardInstance Card { get; private set; }
    public Vector3 HandViewPosition { get;  set; }



    public virtual void Setup(CardInstance card)
    {
        Card = card;
    }
    public virtual void ForceUnselect()
    {
        if (CurrentlySelectedCard != null)
        {
            //card down animate
            CurrentlySelectedCard.transform.DOMoveY(CurrentlySelectedCard.transform.position.y - 1, 0.2f).SetEase(Ease.OutCubic);
            isOneCardSelected = false;
            CurrentlySelectedCard = null;
        }
        
    }
    public virtual void ChooseCard()
    {
        isOneCardSelected = false;
        CurrentlySelectedCard = null;
    }
    public virtual GameObject GetWrapper()
    {
        return wrapper;
    }
    public virtual void OnMouseDown()
    {
        if (PlayView.Instance.playedCards.Contains(this)) return;
        if (isOneCardSelected && CurrentlySelectedCard == this)
        {
            ForceUnselect();
        }
        else
        {
            ForceUnselect();
            this.transform.DOMoveY(this.transform.position.y + 1, 0.2f).SetEase(Ease.OutCubic);
            CurrentlySelectedCard = this;
            isOneCardSelected = true;
        }
    }
    

}
