using UnityEngine;

public class GhostsManager : MonoBehaviour
{
    private static GhostsManager Instance = null;
    public static GhostsManager instance => Instance;

    [Header("Spawn Odds")]
    [SerializeField] private PairTypeOdd[] odds;

    [Header("Data")]
    [SerializeField] private PairTypeData[] datas;
    [SerializeField] private GameObject ghost;

    [Header("Quantity")]
    [SerializeField] private QTTPerNight[] ghostQTTPerNight;

    private int nbGhostThisNight;

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

    public void NewNight()
    {
        int _night = GameManager.instance.GetNight();
        if(_night >= ghostQTTPerNight.Length) _night = ghostQTTPerNight.Length - 1;
        nbGhostThisNight = Random.Range(ghostQTTPerNight[_night - 1].minGhost, ghostQTTPerNight[_night - 1].maxGhost);
        NewGhost();
    }

    public void NewGhost()
    {
        if (nbGhostThisNight <= 0)
        {
            GameManager.instance.SetDay();
            return;
        }
        Souls _type = GetSoulType();
        GhostData _data = GetData(_type);
        GameObject _ghost = Instantiate(ghost);
        _ghost.GetComponent<GhostBehaviour>()?.Init(_data);
        nbGhostThisNight--;
    }

    private GhostData GetData(Souls _type)
    {
        foreach(PairTypeData _pair in datas)
        {
            if(_pair.type == _type)
            {
                int _random = Random.Range(0, _pair.data.Length);
                return _pair.data[_random];
            }
        }
        Debug.Log("Soul type not found, return default one");
        return datas[0].data[0];
    }

    private Souls GetSoulType()
    {
        float _total = 0;
        foreach(PairTypeOdd _pair in odds)
        {
            _total += _pair.odd;
        }
        float _random = Random.Range(0, _total);
        foreach (PairTypeOdd _pair in odds)
        {
            _random -= _pair.odd;
            if (_random <= 0) return _pair.type;
        }
        return odds[odds.Length - 1].type;
    }

}

