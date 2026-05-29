using System;
using System.Collections;
using Mirror;
using Mirror.Examples.Basic;
using UnityEngine;

public class HealthSystem : NetworkBehaviour
{
    public static HealthSystem Instance;
    private void Awake() => Instance = this;
    void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
        ActionSystem.AttachPerformer<SlashGA>(SlashPerformer);
        ActionSystem.AttachPerformer<PeachGA>(PeachPerformer);
        ActionSystem.AttachPerformer<DrinkWineGA>(WinePerformer);
        ActionSystem.AttachPerformer<AskForCardGA>(AskForCardPerformer);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
        ActionSystem.DetachPerformer<SlashGA>();
        ActionSystem.DetachPerformer<DrinkWineGA>();
        ActionSystem.DetachPerformer<PeachGA>();
        ActionSystem.DetachPerformer<AskForCardGA>();
    }

    //performers
    //performers for dealing damage : slash card , champion skill,  ...
    private IEnumerator SlashPerformer(SlashGA dealDamageGA)
    {
        PlayerController receiver = dealDamageGA.Reciever;
        //ask for Dogde card
        AskForCardGA askForCardGA = new AskForCardGA(dealDamageGA.Reciever, "Basic_Dodge", 1, 10f);
        yield return AskForCardPerformer(askForCardGA);
        //check if the slash is dogded or not 
        if (receiver.isPlayedCard && receiver.answeredCards != null && receiver.answeredCards.Count > 0)
        {
            dealDamageGA.isEvaded = true;
        }
        if (dealDamageGA.isEvaded)
        {
            Debug.Log($"{dealDamageGA.Reciever.name} đã né thành công!");
            CardSystem.Instance.RpcClearPlayView();
            yield break; // Ngừng luồng này, KHÔNG chạy xuống code trừ máu nữa
        }
        int finalDamage = dealDamageGA.Amount;
        if (dealDamageGA.Source.isDrunk)
        {
            finalDamage += 1;
            dealDamageGA.Source.isDrunk = false;
            Debug.Log("Sát thương được cường hóa bởi tửu!");
        }
        dealDamageGA.Reciever.currentHP -= finalDamage;
        CardSystem.Instance.RpcClearPlayView();
        //ui ------
        if (dealDamageGA.Reciever.currentHP <= 0)
        {

        }
        yield return null;
    }
    public IEnumerator DodgePerformer(DodgeGA dodgeGA)
    {
        dodgeGA.User.currentHand.Remove(dodgeGA.DodgeCard);
        CardSystem.Instance.RpcPlayCardVisual(dodgeGA.User, dodgeGA.DodgeCard);
        Debug.Log($"{dodgeGA.User.name} đã đánh lá Thiểm thành công! Hủy sát thương!");
        yield return null;
    }

    private IEnumerator PeachPerformer(PeachGA peachGA)
    {
        peachGA.Target.currentHP += peachGA.HealAmount;
        if (peachGA.Target.currentHP > peachGA.Target.maxHP)
        {
            peachGA.Target.currentHP = peachGA.Target.maxHP;
        }
        //Todo: thêm hiệu ứng + máu
        Debug.Log($"{peachGA.User.name} đã dùng Đào hồi máu cho {peachGA.Target.name}! Máu hiện tại: {peachGA.Target.currentHP}/{peachGA.Target.maxHP}");
        CardSystem.Instance.RpcClearPlayView();
        yield return null;
    }
    private IEnumerator WinePerformer(DrinkWineGA drinkWineGA)
    {
        drinkWineGA.User.isDrunk = true;
        drinkWineGA.User.HasUseWineCard = true;
        Debug.Log($"{drinkWineGA.User.name} đã uống tửu! Lá Sát tiếp theo sẽ +1 sát thương");
        CardSystem.Instance.RpcClearPlayView();
        yield return null;
    }

    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGA)
    {
        Debug.Log("Enemy Turn");
        yield return new WaitForSeconds(2f);
        Debug.Log("Enemy Turn end");
        yield return new WaitForSeconds(2f);
    }
    [Server]
    public IEnumerator AskForCardPerformer(AskForCardGA askForCardGA)
    {
        PlayerController target = askForCardGA.Target;
        float timeOut = askForCardGA.TimeOut;
        if (target != null)
        {

            target.isSelecting = true;
            target.isPlayedCard = false;
            target.answeredCards.Clear();
            TargetPromptForCard(target.connectionToClient, askForCardGA.CardID, askForCardGA.Amount, timeOut);
            while (target.isSelecting && timeOut > 0)
            {
                timeOut -= Time.deltaTime;
                yield return null;
            }
            if (timeOut <= 0 && target.isSelecting)
            {
                Debug.Log($"[Server] {target.name} đã hết giờ! Mặc định không đánh bài.");
                target.isSelecting = false;
                TargetForceClosePrompt(target.connectionToClient);
            }

        }
        yield return null;
    }


    [TargetRpc]
    public void TargetPromptForCard(NetworkConnection player, string cardID, int amount, float timeOut)
    {
        PromptCardManager.Instance.AskPlayerForCard(cardID, amount, timeOut);
    }
    [TargetRpc]
    public void TargetForceClosePrompt(NetworkConnection player)
    {
        PromptCardManager.Instance.ForceClosePrompt();
        CardView.ForceUnselectAll();
    }

}
