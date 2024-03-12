using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    const float fpsMeasurePeriod = 0.5f;
    private int m_FpsAccumulator = 0;
    private float m_FpsNextPeriod = 0;
    private int m_CurrentFps;
    const string display = "{0} FPS";

    [SerializeField] private TextMeshProUGUI textFPS;
    [SerializeField] private Button pauseButton;

    private bool showFPS = false;
    private float timeToActivateFPSCounter = 5;
    private float timer = 0;
    private int clickcount = 0;
    private void Start()
    {
        m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
    }
    private void OnEnable()
    {
        pauseButton.onClick.AddListener(OnButtonClick);
    }
    private void OnDisable()
    {
        pauseButton.onClick.RemoveAllListeners();
    }
    void Update()
    {
        if (clickcount >= 5)
        {
            timer += Time.deltaTime;
            if (timer < timeToActivateFPSCounter && clickcount >= 15) 
            {
                showFPS = true;
                textFPS.enabled = showFPS;
            }
        }
        CountFPS();
    }
    private void OnButtonClick() 
    {
        clickcount++;
    }
    private void CountFPS()
    {
        // measure average frames per second
        m_FpsAccumulator++;
        if (Time.realtimeSinceStartup > m_FpsNextPeriod)
        {
            m_CurrentFps = (int)(m_FpsAccumulator / fpsMeasurePeriod);
            m_FpsAccumulator = 0;
            m_FpsNextPeriod += fpsMeasurePeriod;
            textFPS.text = string.Format(display, m_CurrentFps);
        }
    }
}