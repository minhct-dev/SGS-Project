using DG.Tweening.Plugins;
using Mirror.Examples.Basic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayCardButtonUI : MonoBehaviour
{
    public void OnClick()
    {
        if (!InputTargetingSystem.Instance.IsReadyToPlay())
        {
            Debug.Log("Chưa đủ điều kiện đánh bài");
            return;
        }
        CardView playedCard = InputTargetingSystem.Instance.GetSelectedCard();
        uint[] listTargetIds = InputTargetingSystem.Instance.GetTargetIds();
        //Debug.Log(playedCard.Card.Number + " " + playedCard.Card.Suit.ToSymbol());
        if (playedCard == null) return;
        PlayerController player = PlayerController.localPlayer;
        player.CmdPlayCard(new CardInstanceData(playedCard.Card), listTargetIds);
        InputTargetingSystem.Instance.CancelSelection();
        CardView.ChooseCard();
    }
}
