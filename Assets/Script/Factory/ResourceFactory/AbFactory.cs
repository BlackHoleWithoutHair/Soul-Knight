using System.Collections.Generic;
using UnityEngine;

public class AbFactory : IResourceFactory
{
    private AssetBundle AbDatas;
    private AssetBundle AbPlayers;
    public AbFactory()
    {
        AbDatas = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + "datas");
        AbPlayers = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/prefabs/" + "players");
    }
    public T GetScriptableObject<T>() where T : ScriptableObject
    {
        return default(T);
    }
    public List<T> GetResources<T>(string name) where T : UnityEngine.Object
    {
        return null;
    }
    public AudioClip GetAudioClip(string name)
    {
        return null;
    }
    public Sprite GetWeaponSprite(string name)
    {
        return null;
    }
    public Sprite GetSkillSprite(string name)
    {
        return null;
    }
    public Sprite GetMaterialSprite(string name)
    {
        return null;
    }
    public Sprite GetPlantSprite(string name)
    {
        return null;
    }
    public Sprite GetProfileSprite(string name)
    {
        return null;
    }
    public Sprite GetTalentSprite(string name)
    {
        return null;
    }
    public TextAsset GetExcelTextAsset(string name)
    {
        return null;
    }
    public RuntimeAnimatorController GetCharacterAnimatorController(string name)
    {
        return null;
    }
    public RuntimeAnimatorController GetPetAnimatorController(string name)
    {
        return null;
    }
    public GameObject GetPlayer(string type)
    {
        return null;
    }
    public GameObject GetPlayerObj(PlayerType type)
    {
        GameObject obj = AbPlayers.LoadAsset<GameObject>(type.ToString());
        return Object.Instantiate(obj);
    }
    public GameObject GetPanel(string name)
    {
        return null;
    }
    public GameObject GetWeapon(PlayerWeaponType type)
    {
        return null;
    }

    public GameObject GetEnemyWeapon(EnemyWeaponType type)
    {
        return null;
    }
    public GameObject GetPlayerBullet(PlayerBulletType type)
    {
        return null;
    }
    public GameObject GetEnemyBullet(EnemyBulletType type)
    {
        return null;
    }
    public GameObject GetKnifeLight(KnifeLightType type)
    {
        return null;
    }
    public GameObject GetLaser(LaserType type)
    {
        return null;
    }

    public GameObject GetEffect(EffectType type)
    {
        return null;
    }
    public GameObject GetItem(string name)
    {
        return null;
    }
    public GameObject GetEffectBoom(EffectBoomType type)
    {
        return null;
    }
    public GameObject GetEnemy(EnemyType type)
    {
        return null;
    }
    public GameObject GetBoss(BossType type)
    {
        return null;
    }
    public GameObject GetDebuff(BuffType type)
    {
        return null;
    }

    public GameObject GetTreasureBox(TreasureBoxType type)
    {
        return null;
    }


}
