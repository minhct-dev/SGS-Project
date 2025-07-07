public class ToolCardModel : CardModel
{   
    public readonly ToolCardData toolCardData;
    public string Description => toolCardData.Description;
    public ToolCardModel(ToolCardData data) : base(data)
    {
        toolCardData = data;
    }
}
