using UnityEngine;

public class GhostBehaviour : MonoBehaviour
{
    private GhostData data;

    public void Init(GhostData _data)
    {
        data = _data;
    }


    public void OnClick()
    {
        InGameUI.instance.ActiveGhostBox(this);
    }

    private void GhostImpact()
    {
        PlayerController.instance.AddMentalHealth(data.mentalHealth);
        PlayerController.instance.AddKarma(data.karma);
        // Add material section
        switch (data.soulType) // In case of specific behaviour
        {
            case Souls.sad:
                break;

            case Souls.corrupted:
                break;

            case Souls.happy:
                break;

            default:
                Debug.Log("Soul type not found");
                break;
        }
    }

    public void IsChoosed(bool _isChoosed)
    {
        if(_isChoosed)
        {
            // Animation choosed
            GhostImpact();
            GhostsManager.instance.NewGhost();
            Destroy(this.gameObject);
        }
        else
        {
            // Animation go away
            GhostsManager.instance.NewGhost();
            Destroy(this.gameObject);
        }
    }

    public GhostData GetData() { return data; }
}
