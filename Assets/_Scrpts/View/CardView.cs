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
    public static CardView CurrentlySelectedCard { get; private set; }
    private bool isSelected = false;
    public CardInstance Card { get; private set; }
    public Vector3 HandViewPosition { get;  set; }



    public virtual void Setup(CardInstance card)
    {
        Card = card;
    }
    public virtual void ForceUnselect()
    {
        isSelected = false;
        wrapper.SetActive(true);
    }
    public virtual GameObject GetWrapper()
    {
        return wrapper;
     }
    public virtual void OnMouseDown()
    {
        if(PlayView.Instance.playedCards.Contains(this)) return;
        if (isSelected)
        {
            //CardViewHoveSystem.Instance.Hide(this);
            ///wrapper.SetActive(true);
            this.transform.DOMove(HandViewPosition,0.2f).SetEase(Ease.OutCubic);
            isSelected = false;
        }
        else
        {
            //wrapper.SetActive(false);
            Vector3 pos = new(transform.position.x, -5f, 0f);
            //CardViewHoveSystem.Instance.Show(Card, transform.position, pos, this);
            this.transform.DOMove(pos,0.2f).SetEase(Ease.OutCubic);
            isSelected = true;
        }
    }
    

}
