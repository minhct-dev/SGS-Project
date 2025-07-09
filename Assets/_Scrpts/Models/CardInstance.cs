public class CardInstance 
{
    public CardData Data { get; private set; }      // Trỏ đến dữ liệu gốc
    public ToolCardData ToolCardData { get; private set; } 
    public int Number { get; private set; }         // 1–13
    public Suit Suit { get; private set; }          // ♠ ♥ ♦ ♣
    public bool IsFaceUp { get; set; } = true;
    public CardInstance(ToolCardData data, int number, Suit suit)
    {
        Data = data;
        Number = number;
        Suit = suit;
    }
    public CardInstance(BasicCardData data, int number, Suit suit)
    {
        Data = data;
        Number = number;
        Suit = suit;
    }
    public CardType Type => Data.CardType;
}
