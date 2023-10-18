﻿using System.Collections;
using UnityEngine;

public class Basketball : IPlayerUnAccumulateWeapon
{
    public Basketball(GameObject obj, ICharacter player) : base(obj, player)
    {
        m_Attr = AttributeFactory.Instance.GetPlayerWeaponAttr(PlayerWeaponType.Basketball);
        CanBeRotated = false;
        m_GameObject.transform.GetChild(0).localScale = new Vector3(0.8f, 0.8f, 1f);
        m_GameObject.GetComponent<Animator>().enabled = true;
    }

    protected override void OnFire()
    {
        base.OnFire();
        EffectFactory.Instance.GetPlayerBullet(PlayerBulletType.Basketball, m_Attr, FirePoint.transform.position, GetShotRot()).AddToController();
        CoroutinePool.Instance.StartCoroutine(HideWeapon());
    }
    private IEnumerator HideWeapon()
    {
        m_GameObject.SetActive(false);
        yield return new WaitForSeconds(1f / m_Attr.FireRate - 0.1f);
        m_GameObject.SetActive(true);
    }
    //protected override Vector2 GetShotDirection()
    //{
    //    return Camera.main.ScreenToWorldPoint(Input.mousePosition) - m_GameObject.transform.position;
    //}
}
