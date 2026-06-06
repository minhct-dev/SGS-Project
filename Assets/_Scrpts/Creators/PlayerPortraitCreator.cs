using System.Collections.Generic;
using UnityEngine;

public class PlayerPortraitCreator : Singleton<PlayerPortraitCreator>
{
    [SerializeField] private GameObject parentInstantiate;
    public static int currentIndex = 0;
    private static readonly Vector3[] PortraitPositions = {
        new Vector3(0,375,0),
        new Vector3(-455,375,0),
        new Vector3(455,375,0),
        new Vector3(-910,375,0),
        new Vector3(875,375,0),
        new Vector3(-910,0,0),
        new Vector3(875,0,0),
    };
    public PlayerUI CreatePlayerPotrait(PlayerUI prefaps, PlayerController player, int positionIndex)
    {
        if (positionIndex < 0 || positionIndex >= PortraitPositions.Length)
        {
            Debug.LogWarning("Invalid portrait position index.");
            return null;
        }
        //, PortraitPositions[positionIndex], Quaternion.identity
        PlayerUI playerPortrait = Instantiate(prefaps, parentInstantiate.transform);
        playerPortrait.GetComponent<RectTransform>().anchoredPosition = PortraitPositions[positionIndex];
        playerPortrait.assignPlayer = player;
        return playerPortrait;
    }

    public int GetNextAvailableIndex()
    {
        return currentIndex++;
    }

}
