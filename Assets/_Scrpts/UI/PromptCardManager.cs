using IO.Swagger.Model;
using Mirror.Examples.Basic;
using UnityEngine;

public class PromptCardManager : MonoBehaviour
{
    public static PromptCardManager Instance;
    [SerializeField]
    private GameObject promptCardPlayButton;
    [SerializeField]
    private GameObject promptCardCancelButton;
    private string RequestCardId;
    private int Amount;
    private void Awake()
    {
        Instance = this;
    }

    public void AskPlayerForCard(string requestCardId, int amount, float timeOut)
    {
        RequestCardId = requestCardId;
        Amount = amount;
        promptCardPlayButton.SetActive(true);
        promptCardCancelButton.SetActive(true);
    }
    public void OnPromptPlayButtonClicked()
    {
        CardView playedCard = InputTargetingSystem.Instance.GetSelectedCard();
        if (playedCard != null && playedCard.Card.CardID == RequestCardId)
        {
            PlayerController.localPlayer.CmdSubmitRequestedCard(new CardInstanceData(playedCard.Card));
            ForceClosePrompt();
        }
        else Debug.Log("Đây không phải bài cần đánh");
    }
    public void OnPromptCancelButtonClicked()
    {
        PlayerController.localPlayer.CmdCancelSubmitCard();
        ForceClosePrompt();
    }
    public void ForceClosePrompt()
    {
        promptCardPlayButton.SetActive(false);
        promptCardCancelButton.SetActive(false);
    }


}
