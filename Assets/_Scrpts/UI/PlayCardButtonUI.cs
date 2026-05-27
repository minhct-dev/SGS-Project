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
            Debug.Log("Chưa đủ điều kiện đánh bài (Mục tiêu chưa hợp lệ)!");
            return;
        }
        if (CardView.SelectedCards.Count == 0)
        {
            Debug.Log("Bạn chưa chọn lá bài nào!");
            return;
        }
        //Ở thời điểm hiện tại chưa có kĩ năng hay lá bài nào yêu cầu dùng muiltiple choice nên chọn lá bài đầu tiên của mảng sẽ là an toàn nhất
        CardView playedCard = CardView.SelectedCards[0];
        if (!playedCard.IsPlayable())
        {
            Debug.Log("Lá bài này không thể đánh vào lúc này!!!");
            return;
        }
        PlayerController.localPlayer.isWaitingForServer = true;
        uint[] listTargetIds = InputTargetingSystem.Instance.GetTargetIds();

        PlayerController player = PlayerController.localPlayer;
        player.CmdPlayCard(new CardInstanceData(playedCard.Card), listTargetIds);
        InputTargetingSystem.Instance.CancelSelection();
        CardView.ClearSelectionState();
    }
}
