using System.Collections.Generic;
using UnityEngine;

public class PlayerState : IState
{
    protected float hor, ver;
    protected new PlayerStateController m_Controller;
    protected IPlayer player;
    protected IWeapon weapon;
    protected GameObject m_GameObject;
    protected CapsuleCollider2D m_Collider;
    protected Rigidbody2D m_rb;
    protected PlayerAttribute m_Attr;
    protected SpriteRenderer m_Sprite;
    protected Animator m_Animator;

    private const float MIN_MOVE_DISTANCE = 0.01f;
    private ContactFilter2D contactFilter2D;
    private readonly List<RaycastHit2D> raycastHit2DList = new List<RaycastHit2D>();
    private readonly List<RaycastHit2D> tangentRaycastHit2DList = new List<RaycastHit2D>();
    public PlayerState(PlayerStateController controller) : base(controller)
    {
        m_Controller = base.m_Controller as PlayerStateController;
        m_GameObject = m_Controller.GetPlayer().gameObject;
        m_Animator = m_GameObject.transform.GetChild(0).GetComponent<Animator>();
        m_Collider = m_GameObject.transform.Find("Collider").GetComponent<CapsuleCollider2D>();
        m_rb = m_GameObject.GetComponent<Rigidbody2D>();
        player = m_Controller.GetPlayer();
        m_Attr = player.m_Attr;
        contactFilter2D = new ContactFilter2D();
        contactFilter2D.useLayerMask = true;
        contactFilter2D.useTriggers = false;
        contactFilter2D.layerMask = ~LayerMask.GetMask("Pet");
    }
    protected override void StateStart()
    {
        //Debug.Log(this);
    }
    protected override void StateUpdate()
    {
        base.StateUpdate();
        hor = player.m_Input.Horizontal;
        ver = player.m_Input.Vertical;
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
}
