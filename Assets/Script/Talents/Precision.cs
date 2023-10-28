
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Precision:ITalent
{
    public Precision(IPlayer player) : base(player) { }
    public override void OnObtain()
    {
        base.OnObtain();
    }
    public int GetWeaponBaseAngle(PlayerWeaponShareAttribute attr)
    {
        int result = attr.BaseAngle;
        if(attr.BaseAngle>0)
        {
            result = Mathf.Clamp(attr.BaseAngle - 7, 3, 100);
        }
        return result;
    }
    public int GetWeaponScatering(PlayerWeaponShareAttribute attr)
    {
        int result = attr.ScatteringRate;
        if(attr.BaseAngle>0)
        {
            if (7 - (attr.BaseAngle - 3) > 0)
            {
                result = Mathf.Clamp(attr.ScatteringRate - (10 - attr.BaseAngle), 0, 100);
            }
        }
        else
        {
            result = Mathf.Clamp(attr.ScatteringRate - 10, 0, 100);
        }
        return result;
    }
    public int GetWeaponCritical(PlayerWeaponShareAttribute attr)
    {
        int result = attr.CriticalRate;
        if(attr.BaseAngle>0)
        {
            if (7 - (attr.BaseAngle - 3) - attr.ScatteringRate > 0)
            {
                result = attr.CriticalRate - (7 - (attr.BaseAngle - 3) - attr.ScatteringRate);
            }
        }
        else
        {
            if (10 - attr.ScatteringRate > 0)
            {
                result = attr.CriticalRate - (10 - attr.ScatteringRate);
            }
        }
        return result;
    }
    
}
