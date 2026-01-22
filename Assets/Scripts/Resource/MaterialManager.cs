using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class MaterialManager : MonoBehaviour
{
    private static MaterialManager Instance = null;
    public static MaterialManager instance => Instance;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI woodIndicator;
    [SerializeField] private TextMeshProUGUI stoneIndicator;
    [SerializeField] private TextMeshProUGUI tileIndicator;

    [Header("Quantity")]
    [SerializeField] private PairTypeQTT[] resources;
    private Dictionary<Resources, int> resourcesDict;

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
        InitializeResourcesDictionnary();
    }

    private void InitializeResourcesDictionnary()
    {
        resourcesDict = new Dictionary<Resources, int>();
        foreach(PairTypeQTT _pair in resources)
        {
            resourcesDict[_pair.material] = _pair.quantity;
        }
        UpdateUI();
    }

    public bool Buy(Resources[] _resources, int[] _price)
    {
        PairTypeQTT[] _resConcerned;
        for(int i = 0 ; i < _resources.Length ; i++)
        {
            if(!resourcesDict.ContainsKey(_resources[i]) || resourcesDict[_resources[i]] < _price[i])
            {
                return false; //Not enought resources
            }
        }
        for(int i = 0; i < _resources.Length; i++)
        {
            resourcesDict[_resources[i]] -= _price[i];
            UpdateResourceArray(_resources[i], resourcesDict[_resources[i]]);
        }
        return true;
    }

    public void AddResource(Resources[] _resources, int[] _qtt)
    {
        for (int i = 0; i < _resources.Length; i++)
        {
            resourcesDict[_resources[i]] += _qtt[i];
            UpdateResourceArray(_resources[i], resourcesDict[_resources[i]]);
        }
    }

    private void UpdateResourceArray(Resources _type, int _newQTT)
    {
        for (int i = 0; i < resources.Length; i++)
        {
            if(resources[i].material == _type)
            {
                resources[i].quantity = _newQTT;
                break;
            }
        }
        UpdateUI();
    }


    private void UpdateUI()
    {
        woodIndicator.text = resourcesDict[Resources.wood].ToString();
        stoneIndicator.text = resourcesDict[Resources.stone].ToString();
        tileIndicator.text = resourcesDict[Resources.tile].ToString();
    }

}

public enum Resources
{
    wood,
    tile,
    stone
}
