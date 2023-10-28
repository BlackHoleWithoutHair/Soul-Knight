using UnityEngine;
public class ILaser : Item
{
    protected PlayerWeaponShareAttribute m_Attr;
    protected GameObject Spark;
    protected GameObject StartLightCircle;
    protected GameObject EndLightCircle;
    protected LineRenderer line;
    private RaycastHit2D hit;
    public ILaser(GameObject obj, Quaternion dir, PlayerWeaponShareAttribute attr) : base(obj)
    {
        m_Attr = attr;
        m_Rot = dir;
        line = gameObject.GetComponent<LineRenderer>();
        EndLightCircle = gameObject.transform.Find("EndLightCircle").gameObject;
    }
    protected override void Init()
    {
        base.Init();
        gameObject.transform.position = position;
        gameObject.transform.rotation = m_Rot;
        hit = Physics2D.Raycast(position, m_Rot * Vector2.right, 100, LayerMask.GetMask("Obstacle"));
        line.SetPosition(0, position);
        line.SetPosition(1, hit.point);
        line.startWidth = 0.5f;
        line.endWidth = 0.5f;
        EndLightCircle.transform.position = hit.point;
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (line.startWidth > 0)
        {
            line.startWidth -= Time.deltaTime * 2;
            line.endWidth -= Time.deltaTime * 2;
        }
        else
        {
            Remove();
        }
    }
}
