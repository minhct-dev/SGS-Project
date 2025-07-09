using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/ToolCard")]
public class ToolCardData : CardData
{
    [field: SerializeField] public string Description { get; set; }

}
