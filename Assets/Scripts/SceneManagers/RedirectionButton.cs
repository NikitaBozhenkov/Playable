using UnityEngine;
using UnityEngine.UI;

public class RedirectionButton : MonoBehaviour
{
    [SerializeField] private string _url;
    [SerializeField] private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(GoToStore);
    }

    private void GoToStore()
    {
        Application.OpenURL(_url);
    }
}