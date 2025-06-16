using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : SingletonDontDestroy<LoadingSceneManager>
{
    [SerializeField] GameObject _root;
    public async void LoadScene(string sceneName)
    {
        WindowUIManager.Instance.CloseAllWindows();
        _root.SetActive(true);
        await SceneManager.LoadSceneAsync(sceneName);
        _root.SetActive(false);
        WindowUIManager.Instance.CloseAllWindows();
    }
}
