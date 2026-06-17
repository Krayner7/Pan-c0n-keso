using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
public class NetworkUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField ipInput;

    [Header("Panel de conexión")]
    public GameObject menuUI;

    [Header("Gameplay UI")]
    public GameObject gameplayUI;

    public void StartHost()
    {
        bool started = NetworkManager.Singleton.StartHost();

        if (started)
        {
            Debug.Log("BOTON HOST FUNCIONA");
            Debug.Log("HOST iniciado");

            // ocultar menú
            menuUI.SetActive(false);
            gameplayUI.SetActive(true);
        }
    }

    public void StartClient()
    {
        string ip = ipInput.text.Trim();

        if (string.IsNullOrEmpty(ip))
        {
            Debug.LogWarning("Debes escribir una IP");
            return;
        }

        UnityTransport transport =
            NetworkManager.Singleton.GetComponent<UnityTransport>();

        transport.SetConnectionData(ip, 7777);

        bool started = NetworkManager.Singleton.StartClient();

        if (started)
        {
            Debug.Log("Conectando a: " + ip);

            // ocultar menú
            menuUI.SetActive(false);
            gameplayUI.SetActive(true);
        }
    }
}
