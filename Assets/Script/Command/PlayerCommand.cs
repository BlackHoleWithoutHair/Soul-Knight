using UnityEngine;
public class PlayerCommand : Singleton<PlayerCommand>
{
    private PlayerModel model;
    private PlayerCommand()
    {
        model = ModelContainer.Instance.GetModel<PlayerModel>();
    }
    public PlayerShareAttr GetPlayerShareAttr(PlayerType type)
    {
        foreach (PlayerShareAttr attr in model.attrs)
        {
            if (attr.Type == type)
            {
                return attr;
            }
        }
        Debug.Log("PlayerCommand GetPlayerShareAttr " + type + " return null");
        return null;
    }
}
