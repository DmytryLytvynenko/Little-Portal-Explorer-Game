using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVMooving : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.03f;

    private Material material;
    private float offsetX;
    private float offsetY;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }
    private void Update()
    {
        offsetX = Time.time * moveSpeed;
        offsetY = Time.time * moveSpeed;
        material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }
}
