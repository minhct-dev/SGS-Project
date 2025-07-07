using UnityEngine;

public class PlayCardButtonUI : MonoBehaviour
{
    public void OnClick()
    {
        CardInstance playedCard = CardViewHoveSystem.Instance.currentSelectedCard.Card;
        //Debug.Log(playedCard.Number + " " + playedCard.Suit.ToSymbol());
        PlayCardGA playCardGA = new(playedCard);
        ActionSystem.Instance.Perform(playCardGA);
    } 
}
