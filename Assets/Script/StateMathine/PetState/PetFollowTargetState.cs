using Pathfinding;
using System.Collections;
using UnityEngine;

public class PetFollowTargetState : PetState
{
    private Seeker seeker;
    private Path path;
    private int CurrentWayPoint;
    private float nextWayPointDis;
    private bool isPetShouldBackToPlayer;
    public PetFollowTargetState(PetStateController controller) : base(controller) { }
    protected override void StateInit()
    {
        base.StateInit();
        seeker = pet.transform.GetComponent<Seeker>();
        nextWayPointDis = 1f;
    }
    protected override void StateStart()
    {
        base.StateStart();
        isPetShouldBackToPlayer = false;
        m_Animator.SetBool("isWalk", true);
        CoroutinePool.Instance.StartCoroutine(SeekerLoop(), this);
    }
    protected override void StateUpdate()
    {
        base.StateUpdate();
        if (GetDirToTarget(m_Controller.target.gameObject).x > 0)
        {
            pet.isLeft = false;
        }
        else if (GetDirToTarget(m_Controller.target.gameObject).x < 0)
        {
            pet.isLeft = true;
        }
        if (GetDistanceToTarget(m_Controller.target.gameObject) < 4)
        {
            m_Controller.SetOtherState(typeof(PetIdleState));
        }
        MoveToTarget();

    }
    protected override void StateEnd()
    {
        base.StateEnd();
        path = null;
        CurrentWayPoint = 0;
        CoroutinePool.Instance.StopAllCoroutineInObject(this);
    }
    private void MoveToTarget()
    {
        if (path == null) return;
        if (CurrentWayPoint >= path.vectorPath.Count) return;
        pet.transform.position += (path.vectorPath[CurrentWayPoint] - pet.transform.position).normalized * 8f * Time.deltaTime;
        //m_rb.velocity = (path.vectorPath[CurrentWayPoint] - pet.transform.position).normalized * 8;
        //Debug.Log(((path.vectorPath[CurrentWayPoint] - pet.transform.position).normalized * 8).magnitude+" "+ m_rb.velocity.magnitude);
        if (Vector2.Distance(path.vectorPath[CurrentWayPoint], pet.transform.position) < nextWayPointDis)
        {
            CurrentWayPoint++;
        }
    }
    private IEnumerator SeekerLoop()
    {
        while (true)
        {
            if (seeker.IsDone())
            {
                if (GetDistanceToTarget(player.gameObject) < 20 && !isPetShouldBackToPlayer)
                {
                    if (m_Controller.target is IPlayer)
                    {
                        IEnemy r = GetCloseEnemy();
                        if (r != null)
                        {
                            m_Controller.target = r;
                        }
                    }

                    if (m_Controller.target.IsDie && m_Controller.target is IEnemy)
                    {
                        m_Controller.target = player;
                    }
                }
                else
                {
                    isPetShouldBackToPlayer = true;
                    m_Controller.target = player;
                }

                seeker.StartPath(pet.transform.position, m_Controller.target.transform.position, SavePath);
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
        }
    }
}
