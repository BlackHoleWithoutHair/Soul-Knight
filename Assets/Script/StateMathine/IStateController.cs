using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class IStateController
{
    protected Dictionary<string, IState> stateDic;
    public Dictionary<string, IState> StateDic => stateDic;
    protected List<IState> StateList;
    protected IState m_State;
    public bool isStateChange;//����һ֡���Ƿ��Ѿ��仯״̬���Ѿ��仯״̬�Ͳ����ٴα仯״̬��ֱ����һ֡
    public IStateController()
    {
        stateDic = new Dictionary<string, IState>();
        StateList = new List<IState>();
    }
    public virtual void SetOtherState(string name)
    {
        if (!isStateChange)
        {
            if (!stateDic.ContainsKey(name))
            {
                Type type = Type.GetType(name);
                if (type == null)
                {
                    Debug.Log("IStateController �޷���" + name + "ת��Ϊtype");
                    return;
                }
                IState state = (IState)Activator.CreateInstance(type, this);
                stateDic.Add(name, state);
            }
            if (m_State != null)
            {
                m_State.GameExit();
            }
            if (stateDic[name] != GetLast(StateList))
            {
                m_State = stateDic[name];
                m_State?.GameStart();
                StateList.Add(m_State);
            }
            if (StateList.Count == 3)
            {
                StateList.RemoveAt(0);
            }
        }
        isStateChange = true;
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
                    Debug.Log("IStateController �޷���" + type + "ת��Ϊtype");
                    return;
                }
                IState state = (IState)Activator.CreateInstance(type, this);
                stateDic.Add(name, state);
            }
            if (stateDic[name] != GetLast(StateList))
            {
                if (m_State != null)
                {
                    m_State.GameExit();
                }
                m_State = stateDic[name];
                m_State?.GameStart();
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
                    Debug.Log("IStateController �޷���" + type + "ת��Ϊtype");
                    return;
                }
                IState state = (IState)Activator.CreateInstance(type, this);
                stateDic.Add(name, state);
            }
            if (m_State != null)
            {
                m_State.GameExit();
            }
            m_State = stateDic[name];
            m_State?.GameStart();
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
        m_State?.GameExit();
        if (m_State != null)
        {
            CoroutinePool.Instance.StopAllCoroutineInObject(m_State);
        }
        m_State = null;
    }
}

