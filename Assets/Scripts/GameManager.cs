using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance = null;
    public static GameManager instance => Instance;

    private int night = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SetNight();
        GhostsManager.instance.NewNight();
    }

    public void SetDay()
    {
        // Day phase
        Debug.Log("No more ghost left, the sun rise... but the world still dark.");
        night++;
    }

    public void SetNight()
    {
        // Night phase
    }

    public int GetNight() { return night; }
}
