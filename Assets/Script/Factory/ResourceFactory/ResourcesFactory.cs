using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourcesFactory : IResourceFactory
{
    private string DataPath = "Datas/";
    private string ExcelPath = "Excel/";
    private string AudioPath = "Audios/";
    private string ImageWeaponPath = "Images/Weapon/";
    private string ImageSkillPath = "Images/Skill/";
    private string ImageMaterialPath = "Images/Materials/";
    private string ImagePlantsPath = "Images/Seeds/";
    private string ImageProfilePath = "Images/Profile/";
    private string ImageTalentPath = "Images/Talent/";
    private string CharacterAnimatorPath = "Animations/Character";
    private string PetAnimatorPath = "Animations/Pet/";
    private string PlayerPath = "Prefabs/Players/";
    private string EnemyPath = "Prefabs/Enemies/";
    private string BossPath = "Prefabs/Boss/";
    private string PanelPath = "Prefabs/Panels/";
    private string WeaponPath = "Prefabs/Weapons/";
    private string EnemyWeaponPath = "Prefabs/Weapons/EnemyWeapons/";
    private string BulletPath = "Prefabs/Bullets/";
    private string EnemyBulletPath = "Prefabs/Bullets/EnemyBullets/";
    private string LaserPath = "Prefabs/Lasers/";
    private string EffectPath = "Prefabs/Effects/";
    private string ItemPath = "Prefabs/Items/";
    private string EffectBoomPath = "Prefabs/BoomEffect/";
    private string TreasureBoxPath = "Prefabs/Environment/TreasureBox/";
    private Dictionary<PlayerBulletType, GameObject> PlayerBulletDic;
    private Dictionary<EnemyBulletType, GameObject> EnemyBulletDic;
    private Dictionary<EffectType, GameObject> EffectDic;
    private Dictionary<EffectBoomType, GameObject> EffectBoomDic;
    public ResourcesFactory()
    {

        PlayerBulletDic = new Dictionary<PlayerBulletType, GameObject>();
        EnemyBulletDic = new Dictionary<EnemyBulletType, GameObject>();
        EffectDic = new Dictionary<EffectType, GameObject>();
        EffectBoomDic = new Dictionary<EffectBoomType, GameObject>();
    }
    public T GetScriptableObject<T>() where T : ScriptableObject
    {
        switch (typeof(T).Name)
        {
            case nameof(PlayerScriptableObject):
                return Resources.Load<T>(DataPath + "PlayerData");

            case nameof(EnemyScriptableObject):
                return Resources.Load<T>(DataPath + "EnemyData");

            case nameof(WeaponScriptableObject):
                return Resources.Load<T>(DataPath + "WeaponData");

            case nameof(EnemyWeaponScriptableObject):
                return Resources.Load<T>(DataPath + "EnemyWeaponData");

            case nameof(SkillScriptableObject):
                return Resources.Load<T>(DataPath + "SkillData");

            case nameof(TipScriptableObject):
                return Resources.Load<T>(DataPath + "TipData");

            case nameof(PlayerDialogueScriptableObject):
                return Resources.Load<T>(DataPath + "PlayerDialogue");

            case nameof(BuffScriptableObject):
                return Resources.Load<T>(DataPath + "BuffData");

            case nameof(CompositionScriptableObject):
                return Resources.Load<T>(DataPath + "CompositionData");

            case nameof(LanguageScriptableObject):
                return Resources.Load<T>(DataPath + "LanguageData");

            case nameof(PlantScriptableObject):
                return Resources.Load<T>(DataPath + "PlantData");
            case nameof(BossScriptableObject):
                return Resources.Load<T>(DataPath + "BossData");

            default:
                return default(T);
        }
    }
    public List<T> GetResources<T>(string name) where T : UnityEngine.Object
    {
        return Resources.LoadAll<T>("").Where(r => r.name == name).ToList();
    }
    public AudioClip GetAudioClip(string name)
    {
        return Resources.Load<AudioClip>(AudioPath + name);
    }
    public Sprite GetWeaponSprite(string name)
    {
        return Resources.Load<Sprite>(ImageWeaponPath + name);
    }
    public Sprite GetSkillSprite(string name)
    {
        return Resources.Load<Sprite>(ImageSkillPath + name);
    }
    public Sprite GetMaterialSprite(string name)
    {
        return Resources.Load<Sprite>(ImageMaterialPath + name);
    }
    public Sprite GetPlantSprite(string name)
    {
        return Resources.Load<Sprite>(ImagePlantsPath + name);
    }
    public Sprite GetProfileSprite(string name)
    {
        return Resources.Load<Sprite>(ImageProfilePath+name);
    }
    public Sprite GetTalentSprite(string name)
    {
        return Resources.Load<Sprite>(ImageTalentPath + name);
    }
    public TextAsset GetExcelTextAsset(string name)
    {
        return Resources.Load<TextAsset>(ExcelPath + name);
    }
    public RuntimeAnimatorController GetCharacterAnimatorController(string name)
    {
        return Resources.LoadAll<RuntimeAnimatorController>(CharacterAnimatorPath)?.Where(x => x.name.Equals(name)).ToArray()[0];
    }
    public RuntimeAnimatorController GetPetAnimatorController(string name)
    {
        return Resources.LoadAll<RuntimeAnimatorController>(PetAnimatorPath)?.Where(x => x.name.Equals(name)).ToArray()[0];
    }

    public GameObject GetPlayer(string type)
    {
        return Resources.Load<GameObject>(PlayerPath + type);
    }
    public GameObject GetPanel(string name)
    {
        return Resources.Load<GameObject>(PanelPath + name);
    }
    public GameObject GetWeapon(PlayerWeaponType type)
    {
        GameObject obj = Resources.Load<GameObject>(WeaponPath + type);
        if (obj == null)
        {
            Debug.Log("ResourcesFactory GetWeapon(" + type + ") return null");
        }
        return obj;
    }
    public GameObject GetEnemyWeapon(EnemyWeaponType type)
    {
        return Resources.Load<GameObject>(EnemyWeaponPath + type);
    }
    public GameObject GetPlayerBullet(PlayerBulletType type)
    {
        if (PlayerBulletDic.TryGetValue(type, out GameObject obj))
        {
            return obj;
        }
        else
        {
            PlayerBulletDic.Add(type, Resources.Load<GameObject>(BulletPath + type));
            return PlayerBulletDic[type];
        }
    }
    public GameObject GetEnemyBullet(EnemyBulletType type)
    {
        if (EnemyBulletDic.TryGetValue(type, out GameObject obj))
        {
            return obj;
        }
        else
        {
            EnemyBulletDic.Add(type, Resources.Load<GameObject>(EnemyBulletPath + type));
            return EnemyBulletDic[type];
        }
    }
    public GameObject GetKnifeLight(KnifeLightType type)
    {
        return Resources.Load<GameObject>(BulletPath + type);
    }
    public GameObject GetLaser(LaserType type)
    {
        return Resources.Load<GameObject>(LaserPath + type);
    }

    public GameObject GetEnemy(EnemyType type)
    {
        return Resources.Load<GameObject>(EnemyPath + type);
    }
    public GameObject GetBoss(BossType type)
    {
        return Resources.Load<GameObject>(BossPath + type);
    }
    public GameObject GetEffect(EffectType type)
    {
        if (EffectDic.TryGetValue(type, out GameObject obj))
        {
            if (obj == null)
            {
                Debug.Log("error");
            }
            return obj;
        }
        else
        {
            EffectDic.Add(type, Resources.Load<GameObject>(EffectPath + type));
            return EffectDic[type];
        }
    }
    public GameObject GetItem(string name)
    {
        return Resources.Load<GameObject>(ItemPath + name);
    }
    public GameObject GetEffectBoom(EffectBoomType type)
    {
        if (EffectBoomDic.TryGetValue(type, out GameObject obj))
        {
            return obj;
        }
        else
        {
            EffectBoomDic.Add(type, Resources.Load<GameObject>(EffectBoomPath + type));
            return EffectBoomDic[type];
        }
    }

    public GameObject GetDebuff(BuffType type)
    {
        return Resources.Load<GameObject>(EffectPath + type);
    }

    public GameObject GetTreasureBox(TreasureBoxType type)
    {
        return Resources.Load<GameObject>(TreasureBoxPath + type + "TreasureBox");
    }
}
