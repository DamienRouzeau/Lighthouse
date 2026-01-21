using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Cinemachine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance = null;
    public static GameManager instance => Instance;

    private int night = 1;
    private bool isNight;
    private UnityEvent nightEvent = new UnityEvent();
    private UnityEvent dayEvent = new UnityEvent();

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera nightCam;
    [SerializeField] private CinemachineVirtualCamera dayCam;

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
    }

    public void SetDay()
    {
        // Day phase
        Debug.Log("No more ghost left, the sun rise... but the world still dark.");
        night++;
        UseCamera(dayCam);
        isNight = false;
        dayEvent?.Invoke();
    }

    public void SetNight()
    {
        // Night phase
        UseCamera(nightCam);
        isNight = true;
        nightEvent?.Invoke();
        GhostsManager.instance.NewNight();
    }

    private void UseCamera(CinemachineVirtualCamera _cam)
    {
        nightCam.Priority = 0;
        dayCam.Priority = 0;
        _cam.Priority = 1;
    }

    #region Events
    // Night
    public void SubscribeNight(UnityAction _fonction)
    {
        nightEvent.AddListener(_fonction);
    }
    public void UnsubscribeNight(UnityAction _fonction)
    {
        nightEvent.RemoveListener(_fonction);
    }

    // Day
    public void SubscribeDay(UnityAction _fonction)
    {
        dayEvent.AddListener(_fonction);
    }
    public void UnsubscribeDay(UnityAction _fonction)
    {
        dayEvent.RemoveListener(_fonction);
    }
    #endregion

    #region Getter
    public int GetNight() { return night; }
    public bool IsNight() { return isNight; }
    #endregion
}
