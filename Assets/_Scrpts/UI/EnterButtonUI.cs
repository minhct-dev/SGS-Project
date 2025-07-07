using UnityEngine;

public class EnterButtonUI : MonoBehaviour
{
    public void OnClick()
    {
        EnemyTurnGA enemyTurnGA = new();
        ActionSystem.Instance.Perform(enemyTurnGA);
    } 
}
