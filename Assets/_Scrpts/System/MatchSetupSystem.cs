using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    private void Start()
    {
        CardSystem.Instance.Setup();
        DrawCardGA drawCardGA = new(4);
        ActionSystem.Instance.Perform(drawCardGA);
    } 

}
