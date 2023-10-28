using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemyState : IState
{
    protected enum EnemyCondition
    {
        Roaming,
        ChaseTarget,
        Attack,
    }
    protected new EnemyStateController m_Controller;
    protected GameObject m_GameObject;
    protected GameObject m_HitPlayerBox;
    protected CapsuleCollider2D m_Collider;
    protected IPlayer player;
    protected Animator m_Animator;
    protected Rigidbody2D m_rb;
    protected GameObject AttackPlayerPoint;
    protected GameObject FindPlayerPoint;
    protected GameObject Exclamation;
    protected EnemyAttribute m_Attr;
    protected IEmployeeEnemy m_Character;
    protected EnemyCondition m_State;

    private const float MIN_MOVE_DISTANCE = 0.001f;
    private ContactFilter2D contactFilter2D;
    private readonly List<RaycastHit2D> raycastHit2DList = new List<RaycastHit2D>();
    private readonly List<RaycastHit2D> tangentRaycastHit2DList = new List<RaycastHit2D>();

    private Seeker seeker;
    private Path path;
    private Vector2 OldTargetPos;
    private Vector2 TargetPos;
    private int CurrentWayPoint;
    private float NextWayPointDis;
    protected bool isReachTarget;
    public EnemyState(EnemyStateController controller) : base(controller)
    {
        isReachTarget = true;
        NextWayPointDis = 1;
        m_Controller = base.m_Controller as EnemyStateController;
        m_Character = m_Controller.GetCharacter();
        m_Attr = m_Character.m_Attr;
        m_GameObject = m_Character.gameObject;
        seeker = m_GameObject.GetComponent<Seeker>();
        m_HitPlayerBox = m_GameObject.transform.Find("HitPlayerBox")?.gameObject;
        Exclamation = m_GameObject.transform.Find("Exclamation")?.gameObject;
        m_Collider = m_GameObject.transform.Find("Collider")?.GetComponent<CapsuleCollider2D>();
        if (ModelContainer.Instance.GetModel<SceneModel>().sceneName == SceneName.MiddleScene)
        {
            player = GameMediator.Instance.GetController<PlayerController>().Player;
        }
        else
        {
            player = GameMediator.Instance.GetController<PlayerController>().Player;
        }
        AttackPlayerPoint = m_GameObject.transform.Find("AttackPoint")?.gameObject;
        FindPlayerPoint = m_GameObject.transform.Find("FindPlayerPoint")?.gameObject;
        m_Animator = m_GameObject.GetComponent<Animator>();
        m_rb = m_GameObject.GetComponent<Rigidbody2D>();

    }
    protected override void StateStart()
    {
        base.StateStart();
        //Debug.Log(this);
    }
    protected float GetAttackDistance()
    {
        return Vector2.Distance(AttackPlayerPoint.transform.position, m_GameObject.transform.position);
    }
    protected bool IsInAttackDistance()
    {
        if (Vector2.Distance(player.gameObject.transform.position, m_GameObject.transform.position) < GetAttackDistance())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected float GetFindPlayerDistance()
    {
        return Vector2.Distance(FindPlayerPoint.transform.position, m_GameObject.transform.position);
    }
    protected bool IsFindPlayer()
    {
        if (Vector2.Distance(player.gameObject.transform.position, m_GameObject.transform.position) < GetFindPlayerDistance())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected IEnumerator Warning(float time, UnityAction action)
    {
        if (Exclamation != null)
        {
            Exclamation.SetActive(true);
        }
        yield return new WaitForSeconds(time);
        Exclamation.SetActive(false);
        action.Invoke();
    }
    protected void StartSeekerLoop()
    {
        CoroutinePool.Instance.StartCoroutine(SeekerLoop(), this);
    }
    protected void StopSeekerLoop()
    {
        CoroutinePool.Instance.StopAllCoroutineInObject(this);
    }
    protected void MoveToTarget(Vector2 pos)
    {
        TargetPos = pos;
        if (path == null) return;
        if (isReachTarget) return;
        Vector3 dir = (path.vectorPath[CurrentWayPoint] - m_GameObject.transform.position).normalized;
        m_GameObject.transform.position += dir * m_Attr.m_ShareAttr.Speed * Time.deltaTime;

        if (Vector2.Distance(path.vectorPath[CurrentWayPoint], m_GameObject.transform.position) < NextWayPointDis)
        {
            CurrentWayPoint++;
        }
        if (CurrentWayPoint >= path.vectorPath.Count)
        {
            isReachTarget = true;
        }
        SetDir(dir);
    }
    private IEnumerator SeekerLoop()
    {
        while (true)
        {
            if (seeker.IsDone())
            {
                if (OldTargetPos != TargetPos)
                {
                    OldTargetPos = TargetPos;
                    seeker.StartPath(m_GameObject.transform.position, TargetPos, SavePath);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void SavePath(Path path)
    {
        if (!path.error)
        {
            this.path = path;
            CurrentWayPoint = 0;
            isReachTarget = false;
        }
    }
    protected void Movement(Vector2 velocity)
    {
        if (velocity == Vector2.zero)
            return;
        if (velocity.magnitude < MIN_MOVE_DISTANCE)
            return;

        Vector2 updateDeltaPosition = Vector2.zero;

        float distance = velocity.magnitude * Time.deltaTime;
        Vector2 direction = velocity.normalized;

        m_Collider.Cast(direction, contactFilter2D, raycastHit2DList, distance);

        Vector2 finalDirection = direction;
        float finalDistance = distance;

        foreach (var hit in raycastHit2DList)
        {
            finalDistance = hit.distance;

            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.white);
            Debug.DrawLine(hit.point, hit.point + direction, Color.yellow);

            float projection = Vector2.Dot(hit.normal, direction);

            if (projection >= 0)
            {
                finalDistance = distance;
            }
            else
            {
                Vector2 tangentDirection = new Vector2(hit.normal.y, -hit.normal.x);

                float tangentDot = Vector2.Dot(tangentDirection, direction);

                if (tangentDot < 0)
                {
                    tangentDirection = -tangentDirection;
                    tangentDot = -tangentDot;
                }

                float tangentDistance = tangentDot * distance;

                if (tangentDot != 0)
                {
                    m_Collider.Cast(tangentDirection, contactFilter2D, tangentRaycastHit2DList, tangentDistance);

                    foreach (var tangentHit in tangentRaycastHit2DList)
                    {
                        Debug.DrawLine(tangentHit.point, tangentHit.point + tangentDirection, Color.magenta);

                        if (Vector2.Dot(tangentHit.normal, tangentDirection) >= 0)
                            continue;

                        if (tangentHit.distance < tangentDistance)
                            tangentDistance = tangentHit.distance;
                    }

                    updateDeltaPosition += tangentDirection * tangentDistance;
                }
            }
        }

        updateDeltaPosition += finalDirection * finalDistance;
        m_GameObject.transform.position += (Vector3)updateDeltaPosition;
    }
    protected bool IsPointInCollider(Vector2 point)
    {
        return Physics2D.OverlapCircleAll(point, 0.5f).Any(x => !x.isTrigger);
    }
    protected bool IsPointInFloor(Vector2 point)
    {
        return GameMediator.Instance.GetController<RoomController>().GetFloorCollider(m_Character.m_Room).OverlapPoint(point);
    }
    protected Vector2 GetRamdomPositionAroundEnemy(float dis)
    {
        Vector2 result = (Vector2)m_GameObject.transform.position + Random.insideUnitCircle * dis;
        while (IsPointInCollider(result) || !IsPointInFloor(result))
        {
            result = (Vector2)m_GameObject.transform.position + Random.insideUnitCircle * dis;
            if (result.magnitude < 3)
            {
                result = result.normalized * 3;
            }
        }
        return result;
    }
    protected void SetDir(Vector2 dir)
    {
        if (dir.x > 0.01)
        {
            m_Character.isLeft = false;
        }
        else if (dir.x < -0.01)
        {
            m_Character.isLeft = true;
        }
    }
}
