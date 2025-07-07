using UnityEngine;

[CreateAssetMenu(menuName = "Data/BasicCard")]
public class BasicCardData : CardData
{
    [field: SerializeField] public override string CardName { get ; set ; }

    [field: SerializeField] public override Sprite Image { get ; set ; }
    [field: SerializeField] public override CardType CardType { get; set; } 
   
} 
