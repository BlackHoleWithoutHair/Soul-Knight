using UnityEngine;

public class PlayerControlInput
{
    public float Horizontal;
    public float Vertical;
    public Vector2 MouseWorldPos;
    public Vector2 CharacterPos;
    public bool isAttackKeyDown;
    public bool isMouseControl;
}

public class PlayerInputModel : AbstractModel
{
    public PlayerControlInput m_InputPack;
    protected override void OnInit()
    {
        m_InputPack = new PlayerControlInput();
    }
}
