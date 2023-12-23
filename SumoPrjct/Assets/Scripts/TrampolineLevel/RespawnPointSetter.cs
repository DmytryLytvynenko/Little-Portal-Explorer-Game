using Sound_Player;
using System;
using UnityEngine;

public class RespawnPointSetter : MonoBehaviour
{
    [SerializeField] private Material materialDefault;
    [SerializeField] private Material materialCurrent;
    [SerializeField] private Respawn playerRespawner;
    [SerializeField] private SoundPlayer soundPlayer;

    public static Action<RespawnPointSetter> CurrentRespawnPointChanged;

    private MeshRenderer meshRenderer;
    private bool current = false; 

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
        if (current) return;

        if (other.GetComponent<HeroController>())
        {
            current = true;
            soundPlayer.PlaySound(SoundName.GameSaved);
            CurrentRespawnPointChanged?.Invoke(this);
            meshRenderer.material = materialCurrent;
            playerRespawner.respawnPoint = transform;
        }
    }

    private void OnCurrentRespawnPointChanged(RespawnPointSetter currentRespawnPointSetter)
    {
        if (this == currentRespawnPointSetter) return;

        current = false;
        meshRenderer.material = materialDefault;
    }
}
