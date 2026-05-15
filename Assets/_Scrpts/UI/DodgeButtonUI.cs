using Mirror.Examples.Basic;
using UnityEngine;

public class DodgeButtonUI : MonoBehaviour
{
    public static DodgeButtonUI Instance;

    [SerializeField] private GameObject playDodgeButtonUI;
    [SerializeField] private GameObject CancelButtonUI;
    public void OnConfirmDodgeButtonClicked()
    {

        // Báo lên Server: Có dùng bài, đây là lá bài tui dùng!
        CardView playedCard = InputTargetingSystem.Instance.GetSelectedCard();
        if (playedCard != null && playedCard.Card.Name == "Thiểm")
        {
            PlayerController.localPlayer.CmdAnswerDodge(true, new CardInstanceData(playedCard.Card));
            SetDodgeUIVisible(false);
        }
        else Debug.Log("Đây không phải thiểm");


        // Ẩn UI đi
        // ...
    }
    public void OnCancelButtonClicked()
    {
        // Báo lên Server: Không dùng bài, chém tui đi!
        PlayerController.localPlayer.CmdAnswerDodge(false, default);

        // Ẩn UI đi
        SetDodgeUIVisible(false);
        // ...
    }

    public void SetDodgeUIVisible(bool isVisible)
    {
        playDodgeButtonUI.SetActive(isVisible);
        CancelButtonUI.SetActive(isVisible);
    }
}
