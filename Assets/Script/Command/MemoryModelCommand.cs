public class MemoryModelCommand : Singleton<MemoryModelCommand>
{
    private MemoryModel model;
    private MemoryModelCommand()
    {
        model = ModelContainer.Instance.GetModel<MemoryModel>();
    }
    public void AddMoney(int addition)
    {
        model.Money += addition;
    }
    public void EnterOnlineMode()
    {
        model.isOnlineMode.Value = true;
    }
    public void ExitOnlineMode()
    {
        model.isOnlineMode.Value = false;
    }
    public void InitMemoryModel()
    {
        model.PlayerAttr = null;
        model.Money = 0;
        model.Stage = 1;
    }
    public int GetBigStage()
    {
        return (model.Stage - 1) / 5 + 1;
    }
    public int GetSmallStage()
    {
        return model.Stage - (GetBigStage() - 1) * 5;
    }
}
