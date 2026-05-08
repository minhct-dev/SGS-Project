using System;
using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
        ActionSystem.AttachPerformer<DealDamageGA>(DealDamagePeformer);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
        ActionSystem.DetachPerformer<DealDamageGA>();
    }

    //performers
    //performers for dealing damage : slash card , champion skill,  ...
    private IEnumerator DealDamagePeformer(DealDamageGA dealDamageGA)
    {
        //ask for Dogde card

        //check if the slash is dogded or not 
        if (dealDamageGA.isEvaded)
        {
            Debug.Log($"{dealDamageGA.Reciever.name} đã né thành công!");
            yield break;
        }
        dealDamageGA.Reciever.currentHP -= dealDamageGA.Amount;
        if (dealDamageGA.Reciever.currentHP <= 0)
        {

        }

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
