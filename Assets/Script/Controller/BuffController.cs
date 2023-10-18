using System.Collections.Generic;

public class BuffController : AbstractController
{
    private Dictionary<ICharacter, List<IBuff>> BuffDic;
    public BuffController()
    {
        BuffDic = new Dictionary<ICharacter, List<IBuff>>();
    }
    protected override void AlwaysUpdate()
    {
        base.AlwaysUpdate();
        foreach (ICharacter character in BuffDic.Keys)
        {
            for (int i = 0; i < BuffDic[character].Count; i++)
            {
                if (BuffDic[character][i].CanBeRemove)
                {
                    BuffDic[character][i].OnRemove();
                    BuffDic[character].RemoveAt(i);
                }
            }
        }
    }
    public void AddBuff(ICharacter owner, BuffType type)
    {
        if (!BuffDic.TryGetValue(owner, out List<IBuff> list))
        {
            list = new List<IBuff>();
            BuffDic.Add(owner, list);
        }
        if (list.Count == 0)
        {
            CreateBuff(type, owner, list);
            return;
        }
        IBuff buff = list.Find((x) => x.m_Attr.buffType == type);
        if (buff != null)
        {
            if (buff.m_Attr.stackType == BuffStackType.OnlyStackLevel)
            {
                buff.level++;
            }
        }
        else
        {
            CreateBuff(type, owner, list);
        }
    }
    public void RemoveBuff(BuffType type, ICharacter owner)
    {
        if (BuffDic.TryGetValue(owner, out List<IBuff> list))
        {
            int index = list.FindIndex((x) => x.m_Attr.buffType == type);
            if (index >= 0)
            {
                list.RemoveAt(index);
            }
        }
    }
    public bool ContainBuff(BuffType type, ICharacter owner)
    {
        return BuffDic.TryGetValue(owner, out List<IBuff> list) ? list.Find((x) => x.m_Attr.buffType == type) != null : false;
    }
    private void CreateBuff(BuffType type, ICharacter owner, List<IBuff> list)
    {
        IBuff buff = null;
        switch (type)
        {
            case BuffType.None:
                break;
            case BuffType.Burn:
                buff = new BuffBurn(owner);
                break;
            case BuffType.Freeze:
                buff = new BuffFreeze(owner);
                break;
            case BuffType.Dizzy:
                buff = new BuffDizzy(owner);
                break;
            case BuffType.Poisoning:
                buff = new BuffPoisoning(owner);
                break;
            default: break;

        }
        if (buff != null)
        {
            buff.m_Attr = AttributeFactory.Instance.GetBuffData(type);
            list.Add(buff);
            buff.Execute();
        }
    }
}
