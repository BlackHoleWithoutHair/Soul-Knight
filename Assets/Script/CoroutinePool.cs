using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoroutinePool : MonoBehaviour
{
    private Dictionary<object, List<Coroutine>> CoroutineDic;
    public static CoroutinePool Instance
    {
        get
        {
            if (GameObject.Find("CoroutinePool") == null)
            {
                GameObject obj = new GameObject("CoroutinePool");
                obj.AddComponent<CoroutinePool>();
            }
            return GameObject.Find("CoroutinePool").GetComponent<CoroutinePool>();
        }
    }
    public CoroutinePool()
    {
        CoroutineDic = new Dictionary<object, List<Coroutine>>();
    }
    public Coroutine StartCoroutine(IEnumerator routine, object obj)
    {
        Coroutine coroutine = base.StartCoroutine(routine);
        if (CoroutineDic.ContainsKey(obj))
        {
            CoroutineDic[obj].Add(coroutine);
        }
        else
        {
            CoroutineDic.Add(obj, new List<Coroutine>());
            CoroutineDic[obj].Add(coroutine);
        }
        return coroutine;
    }
    public void StopAllCoroutineInObject(object obj)
    {
        if (!CoroutineDic.ContainsKey(obj))
        {
            return;
        }
        foreach (Coroutine coroutine in CoroutineDic[obj])
        {
            if (coroutine != null)
            {               
                StopCoroutine(coroutine);
            }
        }
        CoroutineDic[obj].Clear();
    }
    public void DelayInvoke(UnityAction action, float time)
    {
        StartCoroutine(WaitForInvoke(action, time));
    }
    private IEnumerator WaitForInvoke(UnityAction action, float time)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }
    public void StartAnimatorCallback(Animator anim, string stateName, System.Action action)
    {
        StartCoroutine(AnimatorCallback(anim, stateName, action));
    }
    public void StartAnimatorCallback(Animator anim, string stateName, System.Action action, object obj)
    {
        StartCoroutine(AnimatorCallback(anim, stateName, action), this);
    }
    private IEnumerator AnimatorCallback(Animator anim, string stateName, System.Action callback)
    {
        while (true)
        {
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
            if (!info.IsName(stateName) || !anim.enabled || !anim.gameObject.activeSelf)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(info.length);
                callback.Invoke();
                yield break;
            }
        }
    }
}
