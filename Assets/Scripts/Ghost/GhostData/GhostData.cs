using UnityEngine;

[CreateAssetMenu(fileName = "GhostData", menuName = "ScriptableObject/Ghost/GhostData", order = 1)]
public class GhostData : ScriptableObject
{
    public string ghostName;
    public string[] dialog;
    public float karma;
    public float mentalHealth;
    public Souls soulType;

}

public enum Souls
{
    sad,
    corrupted,
    happy,
    chief,
    eco,
    mean,
    trickster
}
