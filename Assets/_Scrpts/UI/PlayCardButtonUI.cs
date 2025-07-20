using Mirror.Examples.Basic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayCardButtonUI : MonoBehaviour
{
    public void OnClick()
    {
        CardView playedCard = CardView.CurrentlySelectedCard;
        //Debug.Log(playedCard.Card.Number + " " + playedCard.Card.Suit.ToSymbol());
        if (playedCard == null) return;
        PlayerController player = PlayerController.localPlayer;
        player.CmdPlayCard(new CardInstanceData(playedCard.Card));
        CardView.ChooseCard();
    } 
}
