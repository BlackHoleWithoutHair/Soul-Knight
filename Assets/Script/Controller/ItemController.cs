using System.Collections.Generic;

public class ItemController : AbstractController
{
    private List<Item> items;
    public ItemController()
    {
        items = new List<Item>();
    }
    protected override void AlwaysUpdate()
    {
        base.AlwaysUpdate();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].isAlreadyRemove == false)
            {
                items[i].GameUpdate();
            }
            else
            {
                items.RemoveAt(i);
            }
        }
    }
    public void AddItem(Item effect)
    {
        effect.OnEnter();
        items.Add(effect);
    }
}

