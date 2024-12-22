using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitioner : MonoBehaviour
{
    [SerializeField] private Image _progressBar;
    
    private AsyncOperation _loadSceneAsync;
    
    public void SwitchScene(string sceneName)
    {
        _loadSceneAsync = SceneManager.LoadSceneAsync(sceneName);
        _progressBar.fillAmount = 0;
    }

    private void Update()
    {
        if(_loadSceneAsync == null)
            return;
        
        _progressBar.fillAmount = Mathf.Lerp(_progressBar.fillAmount, _loadSceneAsync.progress,
            Time.deltaTime);
    }
    
}
