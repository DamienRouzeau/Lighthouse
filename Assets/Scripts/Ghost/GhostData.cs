using UnityEngine;

[CreateAssetMenu(fileName = "GhostData", menuName = "ScriptableObject/Ghost", order = 1)]
public class GhostData : ScriptableObject
{
    public string ghostName;
    public string[] dialog;
    public float karma;

}
