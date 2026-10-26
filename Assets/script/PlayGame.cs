using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    [SerializeField] private int sceneIndex;
    public void OnButtonClicked()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
