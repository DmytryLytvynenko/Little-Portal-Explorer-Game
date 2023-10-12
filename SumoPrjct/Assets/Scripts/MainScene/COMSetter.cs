using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMSetter : MonoBehaviour
{
    //Этот скрипт работает только для мих дверей
    [SerializeField] private Transform COM;
    [SerializeField] private Transform referenceScale;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetCOM();
    }

    private void SetCOM()
    {
        rb.centerOfMass = Vector3.Scale(COM.localPosition, referenceScale.localScale * (1f / (referenceScale.localScale.x * referenceScale.localScale.y * referenceScale.localScale.z)));
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetComponent<Rigidbody>().worldCenterOfMass, 0.1f);
    }
}
