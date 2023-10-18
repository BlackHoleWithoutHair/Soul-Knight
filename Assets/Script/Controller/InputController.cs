using UnityEngine;

public class InputController : AbstractController
{
    private bool isMouseControl;
    private PlayerInputModel model;
    private Vector2 dir;
    public InputController()
    {
        model = ModelContainer.Instance.GetModel<PlayerInputModel>();
    }
    protected override void AlwaysUpdate()
    {
        base.AlwaysUpdate();
        if (isMouseControl)
        {
            model.m_InputPack.Horizontal = InputUtility.Instance.GetAxis("Horizontal");
            model.m_InputPack.Vertical = InputUtility.Instance.GetAxis("Vertical");
            model.m_InputPack.MouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            model.m_InputPack.isAttackKeyDown = InputUtility.Instance.GetKey(KeyAction.Attack);
            model.m_InputPack.isMouseControl = true;
        }
        else
        {
            dir.Set(InputUtility.Instance.GetAxis("Horizontal"), InputUtility.Instance.GetAxis("Vertical"));
            if (dir.magnitude != 0)
            {
                model.m_InputPack.MouseWorldPos = dir;
            }
            model.m_InputPack.Horizontal = InputUtility.Instance.GetAxis("Horizontal");
            model.m_InputPack.Vertical = InputUtility.Instance.GetAxis("Vertical");
            model.m_InputPack.isAttackKeyDown = InputUtility.Instance.GetKey(KeyAction.Attack);
            model.m_InputPack.isMouseControl = false;
        }
    }
}
