using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    private static PlayerController Instance = null;
    public static PlayerController instance => Instance;
    [Header("Data")]
    [SerializeField] private float karma;
    [SerializeField] private float mentalHealth;


    [Header("Références")]
    [SerializeField] private Light2D nightLight;
    [SerializeField] private Light2D dayLight;

    [Header("Paramètres de rotation")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private bool smoothRotation = true;

    [Header("Limites de rotation (optionnel)")]
    [SerializeField] private bool useLimits = false;
    [SerializeField] private float minAngle = -90f;
    [SerializeField] private float maxAngle = 90f;

    [Header("Paramètres de la lumière")]
    [SerializeField] private float lightIntensity = 2f;
    [SerializeField] private float lightRadius = 10f;
    [SerializeField] private Color lightColor = Color.white;

    private Camera mainCamera;
    private Transform lightTransform;

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

    void Start()
    {
        mainCamera = Camera.main;

        // Chercher la Light 2D
        if (nightLight == null)
        {
            nightLight = GetComponentInChildren<Light2D>();
        }

        if (nightLight != null)
        {
            lightTransform = nightLight.transform;
            SetupLight();
        }
        else
        {
            Debug.LogError("Aucune Light2D trouvée ! Ajoutez un component Light 2D.");
        }

        GameManager.instance.SubscribeDay(SetDayLight);
        GameManager.instance.SubscribeNight(SetNightLight);
    }

    void Update()
    {
        if (lightTransform != null)
        {
            FollowMouse();
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 _mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        }
    }

    #region Inputs
    public void OnClick(InputValue _value)
    {
        if (!_value.isPressed) return;
        if (InGameUI.instance.IsCurrentlyInDialog())
        {
            InGameUI.instance.NextDialog();
            return;
        }
        Vector2 _mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Collider2D _col = Physics2D.OverlapPoint(_mouseWorldPos);

        if (_col != null)
        {
            Debug.Log("Hit: " + _col.name);

            switch (_col.tag)
            {
                case "Ghost":
                    _col.GetComponent<GhostBehaviour>()?.OnClick();
                    break;

                case "House":
                    break;

                case "GoNightPanel":
                    GameManager.instance.SetNight();
                    break;

                default:
                    Debug.Log("Tag unclickable");
                    break;
            }
        }

    }

    #endregion

    #region Control light

    private void SetDayLight()
    {
        nightLight.gameObject.SetActive(false);
        dayLight.gameObject.SetActive(true);
    }

    private void SetNightLight()
    {
        nightLight.gameObject.SetActive(true);
        dayLight.gameObject.SetActive(false);
    }

    void SetupLight()
    {
        // Configuration de la Light 2D
        nightLight.intensity = lightIntensity;
        nightLight.color = lightColor;

        // Pour un faisceau directionnel, utilisez Freeform ou Point
        if (nightLight.lightType == Light2D.LightType.Point)
        {
            nightLight.pointLightOuterRadius = lightRadius;
        }
        else if (nightLight.lightType == Light2D.LightType.Freeform)
        {
            // Ajustez les points du freeform pour créer un faisceau
            nightLight.shapeLightFalloffSize = 0.5f;
        }

        // Blend mode pour un meilleur effet
        nightLight.blendStyleIndex = 0; // Multiply ou Additive selon votre configuration
    }

    void FollowMouse()
    {
        // Récupérer la position de la souris
        Vector2 _mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 _mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(_mouseScreenPos.x, _mouseScreenPos.y, mainCamera.nearClipPlane));
        _mouseWorldPos.z = transform.position.z;

        // Calculer la direction vers la souris
        Vector3 _direction = transform.position - _mouseWorldPos;

        // Calculer l'angle en degrés
        float _targetAngle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        // Appliquer les limites si activées
        if (useLimits)
        {
            _targetAngle = Mathf.Clamp(_targetAngle, minAngle, maxAngle);
        }

        // Appliquer la rotation
        if (smoothRotation)
        {
            float _currentAngle = lightTransform.localEulerAngles.z;
            float _smoothAngle = Mathf.LerpAngle(_currentAngle, _targetAngle, rotationSpeed * Time.deltaTime);
            lightTransform.localRotation = Quaternion.Euler(0, 0, _smoothAngle);
        }
        else
        {
            lightTransform.localRotation = Quaternion.Euler(0, 0, _targetAngle);
        }
    }

    // Méthodes utilitaires
    public void ToggleLight()
    {
        if (nightLight != null)
        {
            nightLight.enabled = !nightLight.enabled;
        }
    }

    public void SetIntensity(float _intensity)
    {
        lightIntensity = _intensity;
        if (nightLight != null)
        {
            nightLight.intensity = _intensity;
        }
    }

    public void SetRadius(float _radius)
    {
        lightRadius = _radius;
        if (nightLight != null && nightLight.lightType == Light2D.LightType.Point)
        {
            nightLight.pointLightOuterRadius = _radius;
        }
    }
    #endregion

    #region Data
    public void AddMentalHealth(float _add)
    {
        mentalHealth += _add;
    }

    public void AddKarma(float _add)
    {
        karma += _add;
    }
    #endregion
}