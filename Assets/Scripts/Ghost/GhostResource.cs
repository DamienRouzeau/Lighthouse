using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GhostResource : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI qtt;
    [SerializeField] private Image image;

    private void Start()
    {
        InGameUI.instance.SubscribeChoice(SelfDestruct);
    }

    public void SetText(int _qtt)
    {
        qtt.text = _qtt.ToString();
    }

    public void SetSprite(Resources _res)
    {
        image.sprite = ResourcesLibrary.instance.GetSprite(_res);
    }

    public void SelfDestruct()
    {
        InGameUI.instance.UnsubscribeChoice(SelfDestruct);
        Destroy(gameObject);
    }
}
