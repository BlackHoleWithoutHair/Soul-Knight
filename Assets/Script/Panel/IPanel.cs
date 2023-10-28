using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class IPanel
{
    protected GameObject m_GameObject;
    public GameObject gameObject => m_GameObject;
    protected IPanel parent;
    protected List<IPanel> children;
    protected bool m_isSuspend;
    public bool isSuspend => m_isSuspend;
    protected bool isShowPanelAfterExit;
    protected bool isStart;
    protected bool isEnter;
    public IPanel(IPanel parent)
    {
        children = new List<IPanel>();
        this.parent = parent;
    }
    protected virtual void GameStart()
    {
        OnInit();
    }
    public virtual void GameUpdate()
    {
        if (!isStart)
        {
            isStart = true;
            GameStart();
        }
        foreach (IPanel child in children)
        {
            child.GameUpdate();
        }
        if (m_isSuspend == false)
        {
            OnUpdate();
        }
    }

    protected virtual void OnInit()//将所有面板暂停.不能在OnInit中使用EnterPanel,因为递归会导致panel暂停从而无法执行OnUpdate
    {
        OnSuspend();
        if (m_GameObject == null)
        {
            m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
            if (m_GameObject == null)
            {
                m_GameObject = UnityEngine.Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetPanel(GetType().Name), UnityTool.Instance.GetMainCanvas().transform);
            }
            if (m_GameObject == null)
            {
                Debug.Log(this + " m_GameObject is null");
                return;
            }
        }
    }

    protected virtual void OnEnter()
    {
        gameObject.SetActive(true);
        Debug.Log(this);
    }
    protected virtual void OnUpdate()
    {
        if (!isEnter)
        {
            isEnter = true;
            OnEnter();
        }
    }
    protected virtual void OnSuspend()
    {
        m_isSuspend = true;
    }
    protected virtual void OnResume()
    {
        m_isSuspend = false;
    }
    public virtual void OnExit()//检测退出的代码必须放在最后
    {
        if (!isShowPanelAfterExit)
        {
            m_GameObject.SetActive(false);
        }
        OnSuspend();
        parent.isEnter = false;
        parent.OnResume();
    }
    protected virtual void EnterPanel(Type type)
    {
        List<IPanel> panels = children.Where(x => x.GetType() == type).ToList();
        if (panels.Count == 1)
        {
            panels[0].gameObject.SetActive(true);
            panels[0].OnResume();
            panels[0].isEnter = false;
            OnSuspend();
        }
        else
        {
            Debug.Log("存在多个相同类型的Panel，这是不允许的");
        }
    }
    protected T GetPanel<T>() where T : IPanel
    {
        return children.Where(p => p is T).ToArray()[0] as T;
    }
}

