using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(2);
    }
    public void Test()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
