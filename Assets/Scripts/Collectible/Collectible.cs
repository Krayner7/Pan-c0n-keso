using System;
using Unity.Netcode;
using UnityEngine;

public class Collectible : NetworkBehaviour
{
    [Header("Configuración")]
    public int pointValue = 1;

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        if (collected) return;

        PlayerController player =
            other.GetComponent<PlayerController>();

        if (player == null) return;

        // ya lleva algo
        if (player.isCarrying)
            return;

        collected = true;

        // darle el objeto al jugador
        player.isCarrying = true;
        player.carriedValue = pointValue;

        Debug.Log(
            $"Jugador recogió objeto de {pointValue} puntos"
        );

        // desaparecer para todos
        NetworkObject.Despawn();
    }
}
