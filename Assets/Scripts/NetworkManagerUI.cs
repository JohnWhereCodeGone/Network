using UnityEngine;
using Unity.Netcode;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;

    float buttonWidth = 50f;
    float buttonHeight = 15f;

    [SerializeField] float buttonX = 300f;
    [SerializeField] float buttonY = 400f;

    private void calcbuttonPos()
    {
        buttonX = (Screen.width - buttonWidth) / 2;
        buttonY = Screen.height / 2 - buttonHeight;
    }
    private void Awake()
    {
        if (networkManager == null)
        {
            networkManager = GetComponent<NetworkManager>();
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Host"))
        {
            networkManager.StartHost();
        }

        if (GUILayout.Button("Join"))
        {
            networkManager.StartClient();
        }

        if (GUILayout.Button("Quit"))
        {
            Application.Quit();
        }

    }
}
