using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum EventType
{
    OnPetClick,
    OnEnemyHurt,
    OnPlayerHurt,
    OnPlayerClick,
    OnBossCutSceneFinish,
    OnSceneChangeComplete,
    OnPlayerEnterBossRoom,
    OnCameraArriveAtPlayer,
    OnPlayerEnterBattleRoom,


    OnFindRoomResponse,
    OnFinishSelectPlayer,
    OnFindPlayerResponse,
    OnFinishRoomGenerate,
    OnEnterOnlineStartRoomResponse,

    OnWantPlant,
    OnWantForging,
    OnUseLuckyCat,
    OnWantUseFridge,
    OnWantUseSafeBox,
    OnWantUnlockGarden,


    OnPause,
    OnResume,
    OnWantShowNotice,
}
public interface IEventInfo { }
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> action;
    public EventInfo(UnityAction<T> action)
    {
        this.action += action;
    }
}
public class EventInfo : IEventInfo
{
    public UnityAction action;
    public EventInfo(UnityAction action)
    {
        this.action += action;
    }
}
public class EventOnceInfo : IEventInfo
{
    public UnityAction action;
    public EventOnceInfo(UnityAction action)
    {
        this.action += action;
    }

}
public class EventCenter : Singleton<EventCenter>
{
    private Dictionary<EventType, List<IEventInfo>> EventsDic;
    private EventCenter()
    {
        EventsDic = new Dictionary<EventType, List<IEventInfo>>();
    }
    public void RegisterObserver(EventType type, UnityAction action)
    {
        if (EventsDic.ContainsKey(type))
        {
            foreach (IEventInfo info in EventsDic[type])
            {
                if (info is EventInfo)
                {
                    (info as EventInfo).action += action;
                    return;
                }
            }
            IEventInfo i = new EventInfo(action);
            EventsDic[type].Add(i);
        }
        else
        {
            List<IEventInfo> infos = new List<IEventInfo>();
            infos.Add(new EventInfo(action));
            EventsDic.Add(type, infos);
        }
    }
    public void RegisterObserver<T>(EventType type, UnityAction<T> action)
    {
        if (EventsDic.ContainsKey(type))
        {
            foreach (IEventInfo info in EventsDic[type])
            {
                if (info is EventInfo<T>)
                {
                    (info as EventInfo<T>).action += action;
                    return;
                }
            }
            IEventInfo i = new EventInfo<T>(action);
            EventsDic[type].Add(i);
        }
        else
        {
            List<IEventInfo> infos = new List<IEventInfo>();
            infos.Add(new EventInfo<T>(action));
            EventsDic.Add(type, infos);
        }
    }
    public void RegisterObserverOnce(EventType type, UnityAction action)
    {
        if (EventsDic.ContainsKey(type))
        {
            foreach (IEventInfo info in EventsDic[type])
            {
                if (info is EventOnceInfo)
                {
                    (info as EventOnceInfo).action += action;
                    return;
                }
            }
            IEventInfo i = new EventOnceInfo(action);
            EventsDic[type].Add(i);
        }
        else
        {
            List<IEventInfo> infos = new List<IEventInfo>();
            infos.Add(new EventOnceInfo(action));
            EventsDic.Add(type, infos);
        }
    }
    public void RemoveObserver(EventType type, UnityAction action)
    {
        if (EventsDic.ContainsKey(type))
        {
            foreach (IEventInfo info in EventsDic[type])
            {
                if (info is EventInfo)
                {
                    (info as EventInfo).action -= action;
                    return;
                }
            }
        }
    }
    public void RemoveObserver<T>(EventType type, UnityAction<T> action)
    {
        if (EventsDic.ContainsKey(type))
        {
            foreach (IEventInfo info in EventsDic[type])
            {
                if (info is EventInfo<T>)
                {
                    (info as EventInfo<T>).action -= action;
                    return;
                }
            }
        }
    }
    public void NotisfyObserver(EventType type)
    {

        if (EventsDic.ContainsKey(type))
        {
            foreach (IEventInfo info in EventsDic[type])
            {
                if (info is EventInfo)
                {
                    Debug.Log(type);
                    (info as EventInfo).action.Invoke();
                    return;
                }
            }
        }
    }
    public void NotisfyObserver<T>(EventType type, T param)
    {
        if (EventsDic.ContainsKey(type))
        {
            foreach (IEventInfo info in EventsDic[type])
            {
                if (info is EventInfo<T>)
                {
                    Debug.Log(type);
                    (info as EventInfo<T>).action.Invoke(param);
                    return;
                }
            }
        }
    }
    public void NotisfyObserverOnce(EventType type)
    {
        if (EventsDic.ContainsKey(type))
        {
            foreach (IEventInfo info in EventsDic[type])
            {
                if (info is EventOnceInfo)
                {
                    Debug.Log(type);
                    (info as EventOnceInfo).action.Invoke();
                    (info as EventOnceInfo).action -= (info as EventOnceInfo).action;
                    return;
                }
            }
        }
    }
    public void ClearObserver()
    {
        foreach (EventType type in Enum.GetValues(typeof(EventType)))
        {
            if (type != EventType.OnSceneChangeComplete)
            {
                EventsDic.Remove(type);
            }
        }
    }
}
