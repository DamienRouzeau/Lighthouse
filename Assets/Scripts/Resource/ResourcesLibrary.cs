using UnityEngine;
using TMPro;

public class ResourcesLibrary : MonoBehaviour
{
    private static ResourcesLibrary Instance = null;
    public static ResourcesLibrary instance => Instance;

    [SerializeField] private Resources[] resources;
    [SerializeField] private TextMeshProUGUI[] texts;
    [SerializeField] private Sprite[] sprites;

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

    public TextMeshProUGUI GetText(Resources _res)
    {
        for (int i = 0; i < resources.Length; i++)
        {
            if (resources[i] == _res)
            {
                return texts[i];
            }
        }
        Debug.Log("Text not found for the resource " + _res);
        return null;
    }

    public Sprite GetSprite(Resources _res)
    {
        for (int i = 0; i < resources.Length; i++)
        {
            if (resources[i] == _res)
            {
                return sprites[i];
            }
        }
        Debug.Log("Sprite not found for the resource " + _res);
        return null;
    }
}
