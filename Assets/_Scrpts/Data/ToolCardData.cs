using UnityEngine;
[CreateAssetMenu(menuName = "Data/ToolCard")]
public class ToolCardData : CardData
{
    [field: SerializeField] public override string CardName { get; set; }
    [field: SerializeField] public override Sprite Image { get; set; }
    [field: SerializeField] public override CardType CardType { get; set; }
    [field: SerializeField] public string Description { get; set; }
    
    //fix number and suit later ----------------------------------------------
    [field: SerializeField] public int Number { get; private set; } 
    [field: SerializeField] public Suit Suit { get; private set; }
    //------------------------------------------------------------------------
}
