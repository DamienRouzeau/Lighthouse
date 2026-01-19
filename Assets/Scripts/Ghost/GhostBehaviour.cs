using UnityEngine;

public class GhostBehaviour : MonoBehaviour
{
    [SerializeField] private GhostData data;
    public void OnClick()
    {
        InGameUI.instance.ActiveGhostBox(true, data);
    }
}
