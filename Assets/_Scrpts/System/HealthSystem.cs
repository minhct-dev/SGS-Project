using System;
using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
        ActionSystem.AttachPerformer<DealDamageGA>(DealDamagePerformer);
        ActionSystem.AttachPerformer<DodgeGA>(DodgePerformer);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
        ActionSystem.DetachPerformer<DealDamageGA>();
        ActionSystem.DetachPerformer<DodgeGA>();
    }

    //performers
    //performers for dealing damage : slash card , champion skill,  ...
    private IEnumerator DealDamagePerformer(DealDamageGA dealDamageGA)
    {
        //ask for Dogde card
        yield return dealDamageGA.Reciever.AskForDodge();
        //check if the slash is dogded or not 
        if (dealDamageGA.isEvaded)
        {
            Debug.Log($"{dealDamageGA.Reciever.name} đã né thành công!");
            yield break;
        }
        dealDamageGA.Reciever.currentHP -= dealDamageGA.Amount;
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


    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGA)
    {
        Debug.Log("Enemy Turn");
        yield return new WaitForSeconds(2f);
        Debug.Log("Enemy Turn end");
        yield return new WaitForSeconds(2f);
    }
}
