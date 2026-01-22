using UnityEngine;
using TMPro;
using UnityEngine.Events;


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
    [SerializeField] private GameObject ghostResourceSection;
    [SerializeField] private GhostResource ghostResource;

    [Header("Resources")]
    //[SerializeField] private 

    private UnityEvent choiceMadeEvent = new UnityEvent();


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
        ghostResourceSection.SetActive(false);
        choiceSection.SetActive(false);
        ghostDialogBox.SetActive(false);
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
            if (choiceSection.activeInHierarchy) return;
            choiceSection.SetActive(true);
            ghostResourceSection.SetActive(true);
            for(int i = 0; i < currentGhostData.resource.Length; i++)
            {
                GhostResource _ghostRes = Instantiate(ghostResource, ghostResourceSection.transform);
                _ghostRes.SetText(currentGhostData.resourceQTT[i]);
                _ghostRes.SetSprite(currentGhostData.resource[i]);
            }
        }
        else // Next dialog
        {
            ghostDialog.text = currentGhostData.dialog[ghostDialogIndex];
            ghostDialogIndex++;
        }
    }

    public void AcceptGhost(bool _ghostAccepted)
    {
        choiceMadeEvent?.Invoke();
        currentGhost.IsChoosed(_ghostAccepted);
        currentGhost = null;
        currentGhostData = null;
        ghostDialogIndex = 0;
        choiceSection.SetActive(false);
        ghostResourceSection.SetActive(false);
        ghostDialogBox.SetActive(false);
    }

    public void SubscribeChoice(UnityAction _fonction)
    {
        choiceMadeEvent.AddListener(_fonction);
    }
    public void UnsubscribeChoice(UnityAction _fonction)
    {
        choiceMadeEvent.RemoveListener(_fonction);
    }

    public bool IsCurrentlyInDialog()
    {
        if (currentGhost != null) return true;
        return false;
    }
}
