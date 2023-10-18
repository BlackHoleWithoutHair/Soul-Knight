
using UnityEngine;

public class BrownTreasureBox : ITreasureBox
{
    protected override void OnFinishOpen()
    {
        base.OnFinishOpen();
        WeaponFactory.Instance.GetPlayerWeaponObj(GetWeaponType(), transform.Find("WeaponCreatePoint").position);
    }
    private PlayerWeaponType GetWeaponType()
    {
        if (ModelContainer.Instance.GetModel<SceneModel>().sceneName == SceneName.MiddleScene)
        {
            if (!ModelContainer.Instance.GetModel<MemoryModel>().isOnlineMode)
            {
                switch (Random.Range(0, 5))
                {
                    case 0:
                        return PlayerWeaponType.DesertEagle;
                    case 1:
                        return PlayerWeaponType.SnowFoxL;
                    case 2:
                        return PlayerWeaponType.AK47;
                    case 3:
                        return PlayerWeaponType.AssaultRifle;
                    case 4:
                        return PlayerWeaponType.UZI;
                }
            }
            else
            {
                return PlayerWeaponType.PKP;
            }
        }
        else
        {
            return PlayerWeaponType.PKP;
        }

        return PlayerWeaponType.PKP;
    }
}
