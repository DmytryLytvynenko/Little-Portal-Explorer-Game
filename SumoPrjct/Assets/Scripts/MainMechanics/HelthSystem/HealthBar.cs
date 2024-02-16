using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFilling;
    [SerializeField] private HealthControll Health;
    [SerializeField] private Gradient gradient;

    private Camera mainCamera;


    private void Awake()
    {
        Health.HealthChanged += OnHealthChanged;
        mainCamera = Camera.main;
        healthBarFilling.color = gradient.Evaluate(1);
    }
    private void OnDestroy()
    {
        Health.HealthChanged -= OnHealthChanged;
    }


    private void OnHealthChanged(float valueAsPercentage)
    {
        healthBarFilling.fillAmount = valueAsPercentage;
        healthBarFilling.color = gradient.Evaluate(valueAsPercentage);
    }
    private void LateUpdate()
    {
        transform.LookAt(mainCamera.transform.position);
        transform.Rotate(0, 180, 0);
    }
}
