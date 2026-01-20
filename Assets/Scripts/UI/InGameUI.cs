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
    private GhostBehaviour currentGhost;
    private GhostData currentGhostData;
    private int ghostDialogIndex;
    [SerializeField] private GameObject choiceSection;

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

    public void ActiveGhostBox(GhostBehaviour _ghost)
    {
        currentGhost = _ghost;
        currentGhostData = _ghost.GetData();
        ghostDialogIndex = 0;
        ghostDialogBox.SetActive(true);
        ghostDialog.text = currentGhostData.dialog[ghostDialogIndex];
        ghostName.text = currentGhostData.ghostName;
        ghostDialogIndex++;
    }

    public void NextDialog()
    {
        if(ghostDialogIndex >= currentGhostData.dialog.Length) // End of the current dialog
        {
            choiceSection.SetActive(true);
        }
        else // Next dialog
        {
            ghostDialog.text = currentGhostData.dialog[ghostDialogIndex];
            ghostDialogIndex++;
        }
    }

    public void AcceptGhost(bool _ghostAccepted)
    {
        currentGhost.IsChoosed(_ghostAccepted);
        currentGhost = null;
        currentGhostData = null;
        ghostDialogIndex = 0;
        choiceSection.SetActive(false);
        ghostDialogBox.SetActive(false);
    }

    public bool IsCurrentlyInDialog()
    {
        if (currentGhost != null) return true;
        return false;
    }
}
