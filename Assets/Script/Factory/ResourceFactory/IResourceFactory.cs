using System.Collections.Generic;
using UnityEngine;

public interface IResourceFactory
{
    T GetScriptableObject<T>() where T : ScriptableObject;
    List<T> GetResources<T>(string name) where T : UnityEngine.Object;
    RuntimeAnimatorController GetCharacterAnimatorController(string name);
    RuntimeAnimatorController GetPetAnimatorController(string name);
    AudioClip GetAudioClip(string name);
    Sprite GetWeaponSprite(string name);
    Sprite GetSkillSprite(string name);
    Sprite GetMaterialSprite(string name);
    Sprite GetPlantSprite(string name);
    Sprite GetProfileSprite(string name);
    Sprite GetTalentSprite(string name);
    TextAsset GetExcelTextAsset(string name);
    GameObject GetPlayer(string type);
    GameObject GetEnemy(EnemyType type);
    GameObject GetBoss(BossType type);
    GameObject GetPanel(string name);
    GameObject GetEnemyWeapon(EnemyWeaponType type);
    GameObject GetWeapon(PlayerWeaponType type);
    GameObject GetPlayerBullet(PlayerBulletType type);
    GameObject GetEnemyBullet(EnemyBulletType type);
    GameObject GetKnifeLight(KnifeLightType type);
    GameObject GetEffect(EffectType type);
    GameObject GetItem(string name);
    GameObject GetEffectBoom(EffectBoomType type);
    GameObject GetDebuff(BuffType type);
    GameObject GetLaser(LaserType type);
    GameObject GetTreasureBox(TreasureBoxType type);

}
