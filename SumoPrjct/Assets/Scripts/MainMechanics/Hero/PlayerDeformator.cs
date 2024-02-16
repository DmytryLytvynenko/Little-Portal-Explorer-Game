using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDeformator : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform playerBody;
    [SerializeField] private float scaleKoefficirnt;
    [SerializeField] private float rotationKoefficient;


    [SerializeField] private Vector3 scaleDown = new Vector3(1.2f,0.8f,1.2f);
    [SerializeField] private Vector3 scaleUp = new Vector3(0.8f,1.2f, 0.8f);
    private Vector3 defaultScale = Vector3.one;

    private void Awake()
    {
        transform.parent = null;
    }
    void Update()
    {
        Vector3 relativePosition = playerTransform.InverseTransformPoint(transform.position);
        float interpolant = relativePosition.y * scaleKoefficirnt;
        Vector3 scale = Lerp3(scaleDown, defaultScale, scaleUp, interpolant);
        playerBody.localScale = scale;

        Vector3 newRotation = new Vector3(relativePosition.z, 0, -relativePosition.x) * rotationKoefficient;
        playerBody.localEulerAngles = new Vector3(
                Mathf.Clamp(newRotation.x, -50, 50), 
                0, 
                Mathf.Clamp((newRotation.z), -20, 20)
                );
/*        playerBody.localEulerAngles = new Vector3(Mathf.Clamp(relativePosition.z, -20, 20), 0, Mathf.Clamp((-relativePosition.x), -50, 50)) * rotationKoefficient;
*/
    }
    private Vector3 Lerp3(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        if (t < 0)
            return Vector3.LerpUnclamped(a, b, t + 1f);
        else
            return Vector3.LerpUnclamped(b, c, t);
    }
    public void DeactivateDeformator()
    {
        this.enabled = false;
        playerBody.localScale = defaultScale;
        playerBody.localRotation = Quaternion.Euler(0,0,0);
    }
    public void ActivateDeformator()
    {
        this.enabled = true;
    }
}
