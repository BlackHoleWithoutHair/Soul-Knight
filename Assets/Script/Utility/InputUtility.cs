using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class InputUtility : Singleton<InputUtility>
{
    private PlayerInput input;
    private InputUtility()
    {
        input = new PlayerInput();
        if (ModelContainer.Instance.GetModel<ArchiveModel>().KeyData != null)
        {
            //Debug.Log(ArchiveSystem.Instance.KeyData);
            input.PlayerAction.Get().LoadBindingOverridesFromJson(ModelContainer.Instance.GetModel<ArchiveModel>().KeyData);
        }
        input.Enable();
    }
    public void RebindAction(KeyAction action, int index, UnityAction callback)
    {
        InputAction tmpAction = input.PlayerAction.Get().asset[action.ToString()];
        input.Disable();
        int tmpIndex = tmpAction.GetBindingIndexForControl(tmpAction.controls[0]) + index;
        tmpAction.PerformInteractiveRebinding(tmpIndex)
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(op =>
            {
                ModelContainer.Instance.GetModel<ArchiveModel>().KeyData = input.PlayerAction.Get().SaveBindingOverridesAsJson();
                ModelContainer.Instance.GetModel<ArchiveModel>().SaveKeyData();
                callback?.Invoke();
                input.Enable();
                op.Dispose();
            })
            .Start();
    }
    public KeyCode GetKeyCode(KeyAction action, int index = 0)
    {
        InputAction ac = input.PlayerAction.Get().asset[action.ToString()];
        int tmpIndex = ac.GetBindingIndexForControl(ac.controls[0]) + index;
        string path = ac.bindings[tmpIndex].effectivePath;
        if (path == null || path == "")
        {
            return KeyCode.None;
        }
        string s = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        s = s.Replace(" ", "");
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), s);
    }
    public int GetNumOfBindings(KeyAction action)
    {
        return input.PlayerAction.Get().asset[action.ToString()].bindings.Count;
    }
    public float GetAxis(string name)
    {
        float result = 0;
        if (name == "Horizontal")
        {
            if (GetKey(KeyAction.Left) && !GetKey(KeyAction.Right))
            {
                result = -1;
            }
            else if (GetKey(KeyAction.Right) && !GetKey(KeyAction.Left))
            {
                result = 1;
            }
            else
            {
                result = 0;
            }
        }
        if (name == "Vertical")
        {
            if (GetKey(KeyAction.Down) && !GetKey(KeyAction.Up))
            {
                result = -1;
            }
            else if (GetKey(KeyAction.Up) && !GetKey(KeyAction.Down))
            {
                result = 1;
            }
            else
            {
                result = 0;
            }
        }
        return result;
    }
    public bool GetKeyDown(KeyAction action)
    {
        return input.PlayerAction.Get().asset[action.ToString()].WasPressedThisFrame();
    }
    public bool GetKeyUp(KeyAction action)
    {
        return input.PlayerAction.Get().asset[action.ToString()].WasReleasedThisFrame();
    }
    public bool GetKey(KeyAction action)
    {
        return input.PlayerAction.Get().asset[action.ToString()].IsPressed();
    }
}

