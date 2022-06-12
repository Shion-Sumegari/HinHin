using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashController : MonoBehaviour
{
    public GameObject m_LevelLoader;
    [SerializeField] Image _progressImage;
    [SerializeField] float _raiseTime = 0.5f;
    Coroutine _raiseCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.m_LevelLoader = m_LevelLoader;
        StartCoroutine(LoadDataFromServer());
    }

    IEnumerator LoadDataFromServer()
    {
        yield return new WaitUntil(() => { return GameManager.Instance.isPlayerDataLoaded; });
        LogUtils.Log("User's Data Loaded");
        if (_raiseCoroutine != null) StopCoroutine(_raiseCoroutine);
        _raiseCoroutine = StartCoroutine(RaiseProgress(0.25f));
        yield return new WaitForSeconds(2f);
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        asyncOperation.completed += OnFinishLoadScene;
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            LogUtils.Log("Loading progress: " + (asyncOperation.progress * 100) + "%");
            if (_raiseCoroutine != null) StopCoroutine(_raiseCoroutine);
                _raiseCoroutine = StartCoroutine(RaiseProgress(asyncOperation.progress * (1f - _progressImage.fillAmount)));
            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                LogUtils.Log("Press the space bar to continue");
                //Wait to you press the space key to activate the Scene
                GameManager.Instance.BeginTransition();
                yield return new WaitForSeconds(1f);
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    void OnFinishLoadScene(AsyncOperation operation)
    {
        if (operation.isDone)
        {
            GameManager.Instance.uIController.mainCanvas.worldCamera = Camera.main.transform.GetChild(0).GetComponent<Camera>();
        }
    }
    IEnumerator RaiseProgress(float progress)
    {
        float currentAmount = _progressImage.fillAmount;
        var elapsedTime = 0f;
        while(elapsedTime < _raiseTime)
        {
            elapsedTime += Time.deltaTime;
            _progressImage.fillAmount = Mathf.MoveTowards(currentAmount, currentAmount + progress, elapsedTime / _raiseTime);
            yield return new WaitForEndOfFrame();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
