using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DialogueData
{
    public PlayerType PlayerType;
    public List<string> Dialogues;
}
[CreateAssetMenu(fileName = "PlayerDialogue", menuName = "ScriptableObjects/PlayerDialogue")]
public class PlayerDialogueScriptableObject : ScriptableObject
{
    public List<DialogueData> DialogueDatas;
}
