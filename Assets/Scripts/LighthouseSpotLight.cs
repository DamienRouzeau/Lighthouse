using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class LighthouseLight2D : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private Light2D light2D;

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

    void Start()
    {
        mainCamera = Camera.main;

        // Chercher la Light 2D
        if (light2D == null)
        {
            light2D = GetComponentInChildren<Light2D>();
        }

        if (light2D != null)
        {
            lightTransform = light2D.transform;
            SetupLight();
        }
        else
        {
            Debug.LogError("Aucune Light2D trouvée ! Ajoutez un component Light 2D.");
        }
    }

    void SetupLight()
    {
        // Configuration de la Light 2D
        light2D.intensity = lightIntensity;
        light2D.color = lightColor;

        // Pour un faisceau directionnel, utilisez Freeform ou Point
        if (light2D.lightType == Light2D.LightType.Point)
        {
            light2D.pointLightOuterRadius = lightRadius;
        }
        else if (light2D.lightType == Light2D.LightType.Freeform)
        {
            // Ajustez les points du freeform pour créer un faisceau
            light2D.shapeLightFalloffSize = 0.5f;
        }

        // Blend mode pour un meilleur effet
        light2D.blendStyleIndex = 0; // Multiply ou Additive selon votre configuration
    }

    void Update()
    {
        if (lightTransform != null)
        {
            FollowMouse();
        }
    }

    void FollowMouse()
    {
        // Récupérer la position de la souris
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, mainCamera.nearClipPlane));
        mouseWorldPos.z = transform.position.z;

        // Calculer la direction vers la souris
        Vector3 direction = transform.position - mouseWorldPos;

        // Calculer l'angle en degrés
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Appliquer les limites si activées
        if (useLimits)
        {
            targetAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle);
        }

        // Appliquer la rotation
        if (smoothRotation)
        {
            float currentAngle = lightTransform.localEulerAngles.z;
            float smoothAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
            lightTransform.localRotation = Quaternion.Euler(0, 0, smoothAngle);
        }
        else
        {
            lightTransform.localRotation = Quaternion.Euler(0, 0, targetAngle);
        }
    }

    // Méthodes utilitaires
    public void ToggleLight()
    {
        if (light2D != null)
        {
            light2D.enabled = !light2D.enabled;
        }
    }

    public void SetIntensity(float intensity)
    {
        lightIntensity = intensity;
        if (light2D != null)
        {
            light2D.intensity = intensity;
        }
    }

    public void SetRadius(float radius)
    {
        lightRadius = radius;
        if (light2D != null && light2D.lightType == Light2D.LightType.Point)
        {
            light2D.pointLightOuterRadius = radius;
        }
    }
}