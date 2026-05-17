using System;
using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
        ActionSystem.AttachPerformer<SlashGA>(SlashPerformer);
        ActionSystem.AttachPerformer<DodgeGA>(DodgePerformer);
        ActionSystem.AttachPerformer<PeachGA>(PeachPerformer);
        ActionSystem.AttachPerformer<DrinkWineGA>(WinePerformer);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
        ActionSystem.DetachPerformer<SlashGA>();
        ActionSystem.DetachPerformer<DodgeGA>();
        ActionSystem.DetachPerformer<DrinkWineGA>();
    }

    //performers
    //performers for dealing damage : slash card , champion skill,  ...
    private IEnumerator SlashPerformer(SlashGA dealDamageGA)
    {
        //ask for Dogde card
        yield return dealDamageGA.Reciever.AskForDodge();
        //check if the slash is dogded or not 
        if (dealDamageGA.Reciever.isDodgeCardPlayed)
        {
            DodgeGA dodgeGA = new DodgeGA(
                dealDamageGA.Reciever,
                dealDamageGA.Reciever.playedDodgeCard,
                dealDamageGA
            );
            yield return DodgePerformer(dodgeGA);
        }
        if (dealDamageGA.isEvaded)
        {
            Debug.Log($"{dealDamageGA.Reciever.name} đã né thành công!");
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
    private IEnumerator DodgePerformer(DodgeGA dodgeGA)
    {
        dodgeGA.User.currentHand.Remove(dodgeGA.DodgeCard);
        CardSystem.Instance.RpcPlayCardVisual(dodgeGA.User, dodgeGA.DodgeCard);
        dodgeGA.TargetDamageAction.isEvaded = true;
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
}
