using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RestartButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    static void OnClick()
    {
        UserProgress.Current.ClearState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}