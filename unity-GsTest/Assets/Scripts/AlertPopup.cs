using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AlertPopup : MonoBehaviour
{
    public TMP_Text messageTextMesh;
    public GameObject panel;
    public Button closeButton;
    private void Start()
    {
        Close();
    }
    public void ShowPopup(string message)
    {
        panel.SetActive(true);
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(Close);
        messageTextMesh.text = message;
    }
    public void Close()
    {
        panel.SetActive(false);
    }
}
