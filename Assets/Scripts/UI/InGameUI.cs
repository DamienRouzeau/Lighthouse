using UnityEngine;
using TMPro;

public class InGameUI : MonoBehaviour
{
    private static InGameUI Instance = null;
    public static InGameUI instance => Instance;

    [Header("Ghost")]
    [SerializeField] private GameObject ghostDialogBox;
    [SerializeField] private TextMeshProUGUI ghostDialog;
    [SerializeField] private TextMeshProUGUI ghostName;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public void ActiveGhostBox(bool active, GhostData data)
    {
        ghostDialogBox.SetActive(active);
        ghostDialog.text = data.dialog[0];
        ghostName.text = data.ghostName;
    }
}
