using Unity.Netcode;
using UnityEngine;

public class DeliveryZone : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        PlayerController player =
            other.GetComponent<PlayerController>();

        if (player == null)
            return;

        // no lleva nada
        if (!player.isCarrying)
            return;

        // sumar puntos
        player.score.Value += player.carriedValue;

        Debug.Log(
            $"Puntos entregados: {player.carriedValue}"
        );

        // vaciar inventario
        player.isCarrying = false;
        player.carriedValue = 0;

        Debug.Log("Score total: " + player.score.Value);
    }
}
