public class MemoryModel : AbstractModel
{
    public PlayerAttribute PlayerAttr;
    public PetType PetType;
    public string UserName;
    public int Stage;
    public int Money;
    public BindableProperty<bool> isOnlineMode = new BindableProperty<bool>(false);
    protected override void OnInit()
    {
        PetType = PetType.LittleCool;
        Stage = 5;
        Money = 0;
    }
}
