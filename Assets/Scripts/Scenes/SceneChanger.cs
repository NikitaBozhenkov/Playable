using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string _newSceneName;

    public void LoadScene()
    {
        SceneManager.LoadScene(_newSceneName);
    }
}