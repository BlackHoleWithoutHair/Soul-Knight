using System.Collections.Generic;
using UnityEngine;

public enum TriggerType
{
    OnTriggerEnter,
    OnTriggerExit,
}
public class TriggerCenter : Singleton<TriggerCenter>
{
    private class GameObjectInfo
    {
        public TriggerType type;
        public GameObject target;
        public event m_Action action;
        public void Invoke(GameObject obj)
        {
            action(obj);
        }
        public void ClearAction()
        {
            action -= action;
        }
    }
    private class TagInfo
    {
        public TriggerType type;
        public string tag;
        public event m_Action action;
        public void Invoke(GameObject obj)
        {
            action(obj);
        }
        public void ClearAction()
        {
            action -= action;
        }
    }
    private class AnyGameObjectInfo
    {
        public TriggerType type;
        public event m_Action action;
        public void Invoke(GameObject obj)
        {
            action.Invoke(obj);
        }
    }

    private Dictionary<GameObject, List<GameObjectInfo>> GameObjectDic;
    private Dictionary<GameObject, List<TagInfo>> TagDic;
    private Dictionary<GameObject, List<AnyGameObjectInfo>> AnyGameObjectDic;
    public delegate void m_Action(GameObject obj);
    private TriggerCenter()
    {
        GameObjectDic = new Dictionary<GameObject, List<GameObjectInfo>>();
        TagDic = new Dictionary<GameObject, List<TagInfo>>();
        AnyGameObjectDic = new Dictionary<GameObject, List<AnyGameObjectInfo>>();
        EventCenter.Instance.RegisterObserver(EventType.OnSceneChangeComplete, () =>
        {
            ClearObserver();
        });
    }

    public void RegisterObserver(TriggerType type, GameObject origin, GameObject target, m_Action action)
    {
        GameObjectInfo info = new GameObjectInfo();
        info.type = type;
        info.action += action;
        info.target = target;
        if (GameObjectDic.ContainsKey(origin))
        {
            foreach (GameObjectInfo i in GameObjectDic[origin])
            {
                if (i.target == target && i.type == type)
                {
                    i.ClearAction();
                    i.action += action;
                    Debug.Log("<" + origin.name + " " + target.name + ">" + " Already exist");
                    return;
                }
            }
            GameObjectDic[origin].Add(info);
        }
        else
        {
            List<GameObjectInfo> infos = new List<GameObjectInfo>();
            infos.Add(info);
            GameObjectDic.Add(origin, infos);
        }
    }
    public void RegisterObserver(TriggerType type, GameObject origin, string tag, m_Action action)
    {
        TagInfo info = new TagInfo();
        info.type = type;
        info.action += action;
        info.tag = tag;
        if (TagDic.ContainsKey(origin))
        {
            foreach (TagInfo i in TagDic[origin])
            {
                if (i.tag == tag && i.type == type)
                {
                    i.ClearAction();
                    i.action += action;
                    Debug.Log("<" + origin.name + " " + tag + ">" + " Already exist");
                    return;
                }
            }
            TagDic[origin].Add(info);
        }
        else
        {
            List<TagInfo> infos = new List<TagInfo>();
            infos.Add(info);
            TagDic.Add(origin, infos);
        }
    }
    public void RegisterObserver(TriggerType type, GameObject obj, m_Action action)
    {
        AnyGameObjectInfo info = new AnyGameObjectInfo();
        info.type = type;
        info.action += action;
        if (AnyGameObjectDic.ContainsKey(obj))
        {
            foreach (AnyGameObjectInfo i in AnyGameObjectDic[obj])
            {
                if (i.type == type)
                {
                    i.action += action;
                    return;
                }
            }
            AnyGameObjectDic[obj].Add(info);
        }
        else
        {
            List<AnyGameObjectInfo> list = new List<AnyGameObjectInfo>();
            list.Add(info);
            AnyGameObjectDic.Add(obj, list);
        }
    }
    public void RemoveObserver(TriggerType type, GameObject origin)
    {
        if (GameObjectDic.ContainsKey(origin))
        {
            for (int i = 0; i < GameObjectDic[origin].Count; i++)
            {
                if (GameObjectDic[origin][i].type == type)
                {
                    GameObjectDic.Remove(origin);
                    break;
                }
            }
        }
        if (TagDic.ContainsKey(origin))
        {
            for (int i = 0; i < TagDic[origin].Count; i++)
            {
                if (TagDic[origin][i].type == type)
                {
                    TagDic.Remove(origin);
                    break;
                }
            }
        }
    }
    public void RemoveObserver(TriggerType type, GameObject origin, GameObject target)
    {
        if (GameObjectDic.ContainsKey(origin))
        {
            for (int i = 0; i < GameObjectDic[origin].Count; i++)
            {
                if (GameObjectDic[origin][i].type == type && GameObjectDic[origin][i].target == target)
                {
                    GameObjectDic[origin].RemoveAt(i);
                }
            }
        }
    }
    public void ClearObserver()
    {
        GameObjectDic.Clear();
        TagDic.Clear();
    }
    public void NotisfyObserver(TriggerType type, GameObject origin, GameObject target)
    {
        if (GameObjectDic.ContainsKey(origin))
        {
            foreach (GameObjectInfo info in GameObjectDic[origin])
            {
                if (info.target == target && info.type == type)
                {
                    info.Invoke(target);
                    break;
                }
            }
        }
        if (TagDic.ContainsKey(origin))
        {
            foreach (TagInfo info in TagDic[origin])
            {
                if (info.tag == target.tag && info.type == type)
                {
                    info.Invoke(target);
                }
            }
        }
        if (AnyGameObjectDic.ContainsKey(origin))
        {
            foreach (AnyGameObjectInfo info in AnyGameObjectDic[origin])
            {
                if (info.type == type)
                {
                    info.Invoke(target);
                }
            }
        }
    }
}
