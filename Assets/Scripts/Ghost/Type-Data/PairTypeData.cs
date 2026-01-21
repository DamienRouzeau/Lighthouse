using UnityEngine;

[CreateAssetMenu(fileName = "PairTypeData", menuName = "ScriptableObject/Ghost/PairTypeData", order = 3)]
public class PairTypeData : ScriptableObject
{
    public GhostData[] data;
    public Souls type;
}
