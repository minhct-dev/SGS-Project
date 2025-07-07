using System.Collections.Generic;
using UnityEngine;

public class TestSystem : MonoBehaviour
{
    
    [SerializeField] private List<BasicCardData>  deckData;
    private void Start()
    {
        CardSystem.Instance.Setup();
    } 


}
