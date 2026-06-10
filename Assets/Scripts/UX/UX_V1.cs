using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class UX_V1 : MonoBehaviour
{
    [Header("Botones")]
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    [Header("IP")]
    [SerializeField]
    private TMP_InputField ipInput;

    private UnityTransport transport;

    private void Awake()
    {
        Debug.Log("AWAKE");
    }
    private void Start()
    {

        Debug.Log("START 1");
        transport =
            NetworkManager.Singleton
            .GetComponent<UnityTransport>();

        Debug.Log("START 2");

        hostButton.onClick.AddListener
        (
            HostButtonOnClick

        );


        Debug.Log("START 3");

        clientButton.onClick.AddListener(
            ClientButtonOnClick
        );
        Debug.Log("START 4");

        Debug.Log(NetworkManager.Singleton);
    }

    private void HostButtonOnClick()
    {
        Debug.Log("CLICK HOST");
        NetworkManager.Singleton.StartHost();

        gameObject.SetActive(false);
    }

    private void ClientButtonOnClick()
    {

        string ip = ipInput.text.Trim();

        if (string.IsNullOrEmpty(ip))
        {
            Debug.Log("Ingresá una IP");
            return;
        }

        transport.SetConnectionData(ip, 7777);

        NetworkManager.Singleton.StartClient();

        gameObject.SetActive(false);

        Debug.Log("CLIENT CLICK");
    }
}