using System;
using UnityEngine;

public class RespawnPointSetter : MonoBehaviour
{
    [SerializeField] private Material materialDefault;
    [SerializeField] private Material materialCurrent;
    [SerializeField] private Respawn playerRespawner;

    public static Action<RespawnPointSetter> CurrentRespawnPointChanged;

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void OnEnable()
    {
        CurrentRespawnPointChanged += OnCurrentRespawnPointChanged;
    }
    private void OnDisable()
    {
        CurrentRespawnPointChanged -= OnCurrentRespawnPointChanged;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            CurrentRespawnPointChanged?.Invoke(this);
            meshRenderer.material = materialCurrent;
            playerRespawner.respawnPoint = transform;
        }
    }

    private void OnCurrentRespawnPointChanged(RespawnPointSetter currentRespawnPointSetter)
    {
        if (this == currentRespawnPointSetter) return;

        meshRenderer.material = materialDefault;
    }
}
