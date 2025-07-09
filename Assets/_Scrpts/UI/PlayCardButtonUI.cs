using UnityEngine;

public class PlayCardButtonUI : MonoBehaviour
{
    public void OnClick()
    {
        CardView playedCard = CardView.CurrentlySelectedCard;
        //Debug.Log(playedCard.Number + " " + playedCard.Suit.ToSymbol());
        if(playedCard == null) return;
        PlayCardGA playCardGA = new(playedCard.Card);
        ActionSystem.Instance.Perform(playCardGA);
    } 
}
