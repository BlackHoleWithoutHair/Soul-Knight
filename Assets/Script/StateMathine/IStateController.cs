using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class IStateController
{
    protected Dictionary<string, IState> stateDic;
    public Dictionary<string, IState> StateDic => stateDic;
    protected List<IState> StateList;
    protected IState m_State;
    public bool isStateChange;//在这一帧中是否已经变化状态，已经变化状态就不能再次变化状态，直到下一帧
    public IStateController()
    {
        stateDic = new Dictionary<string, IState>();
        StateList = new List<IState>();
    }
    public virtual void SetOtherState(Type type)
    {
        if (!isStateChange)
        {
            string name = type.Name;
            if (!stateDic.ContainsKey(name))
            {
                if (type == null)
                {
                    Debug.Log("IStateController 无法将" + type + "转换为type");
                    return;
                }
                IState state = (IState)Activator.CreateInstance(type, this);
                stateDic.Add(name, state);
            }
            if (stateDic[name] != GetLast(StateList))
            {
                if (m_State != null)
                {
                    m_State.OnExit();
                }
                m_State = stateDic[name];
                StateList.Add(m_State);
            }
            if (StateList.Count == 3)
            {
                StateList.RemoveAt(0);
            }
        }
        isStateChange = true;
    }
    public virtual void SetSelfState(Type type)
    {
        if (!isStateChange)
        {
            string name = type.Name;
            if (!stateDic.ContainsKey(name))
            {
                if (type == null)
                {
                    Debug.Log("IStateController 无法将" + type + "转换为type");
                    return;
                }
                IState state = (IState)Activator.CreateInstance(type, this);
                stateDic.Add(name, state);
            }
            if (m_State != null)
            {
                m_State.OnExit();
            }
            m_State = stateDic[name];
            StateList.Add(m_State);
            if (StateList.Count == 3)
            {
                StateList.RemoveAt(0);
            }
        }
        isStateChange = true;
    }
    public virtual void GameUpdate()
    {
        isStateChange = false;
        m_State?.GameUpdate();
    }
    public virtual IState GetPreviousState()
    {
        IState result = null;
        if (StateList.Count == 1)
        {
            result = StateList[0];
        }
        else if (StateList.Count > 1)
        {
            result = StateList[StateList.Count - 2];
        }
        return result;
    }
    public virtual IState GetCurrentState()
    {
        return m_State;
    }
    private T GetLast<T>(List<T> list)
    {
        if (list.Count > 0)
        {
            return list[list.Count - 1];
        }
        else
        {
            return default;
        }
    }
    public void StopCurrentState()
    {
        m_State?.OnExit();
        if (m_State != null)
        {
            CoroutinePool.Instance.StopAllCoroutineInObject(m_State);
        }
        m_State = null;
    }
}

