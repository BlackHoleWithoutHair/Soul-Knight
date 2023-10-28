using System;
using System.Collections.Generic;
using System.Linq;

public class TalentSystem:AbstractSystem
{
    private Dictionary<IPlayer, List<ITalent>> talentDic;
    protected override void OnInit()
    {
        base.OnInit();
        talentDic=new Dictionary<IPlayer, List<ITalent>>();
    }
    public void AddTalen(TalentType type,IPlayer player)
    {
        if(talentDic.ContainsKey(player))
        {
            talentDic[player].Add(TalentFactory.Instance.GetTalent(type, player));
        }
        else
        {
            talentDic.Add(player, new List<ITalent>() { TalentFactory.Instance.GetTalent(type,player)});
        }
    }
    public void RemoveTalent(TalentType type,IPlayer player)
    {
        if(talentDic.ContainsKey(player))
        {
            for (int i = 0; i < talentDic[player].Count; i++)
            {
                if (talentDic[player][i].type == type)
                {
                    talentDic[player].RemoveAt(i);
                }
            }
        }
    }
    public ITalent GetTalent(TalentType type,IPlayer player)
    {
        if(talentDic.ContainsKey(player))
        {
            ITalent[] results= talentDic[player].Where(talent => talent.type == type).ToArray();
            if(results.Length>0)
            {
                return results[0];
            }
        }
        return null;
    }
    public List<ITalent> GetTalents(IPlayer player)
    {
        if(talentDic.ContainsKey(player))
        {
            return talentDic[player];
        }
        return null;
    }
}