[System.Serializable]
public abstract class Effect
{
    public abstract GameAction GetGameAction(PlayerController user);
}
