using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Event : MonoBehaviour
{
    AsyncOperation async = null;
    public DataManager DATA_MANAGER;

    public void SetActiveScene(string scene)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));

    }

    public void UnLoadScene_Async(string scene)
    {
        StartCoroutine(UnLoadAsync(scene));
    }

    public IEnumerator UnLoadAsync(string scene)
    {
        yield return SceneManager.UnloadSceneAsync(scene);
        Resources.UnloadUnusedAssets();
        //bool hasFound = false;

        //while (!async.isDone && !hasFound)
        //{
        //    for (int i = 0; i < SceneManager.sceneCount; i++)
        //    {

        //        if (SceneManager.GetSceneByName("Base_Scene") == SceneManager.GetSceneAt(i))
        //        {
        //            hasFound = true;
        //            SetActiveScene("Base_Scene");
        //            yield break;
        //        }
        //        //  SceneManager.GetActiveScene
        //        yield return null;

        //    }
        //}
    }
    public void Load_ASync(string scene)
    {
        StartCoroutine(LoadAsyncAdditive(scene));
    }
    public IEnumerator LoadAsyncAdditive(string scene)
    {
        //   DATA_MANAGER.isSceneLoading.isOn = true;

        //  aSyncedScenes.Add(scene);

        async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);//, LoadSceneMode.Additive);
                                                                           //async.priority = 2;

        yield return new WaitUntil(() => !DATA_MANAGER.isSceneLoading.isOn);
        // && async.allowSceneActivation);
        SetActiveScene(scene);
        //SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        //   yield return new WaitForSecondsRealtime(5f);

        //  DATA_MANAGER.isSceneLoading.isOn = true;


        //  onSceneAdd.Invoke();

        // yield return new WaitWhile(() => !async.isDone);

        //DATA_MANAGER.isSceneLoading.isOn = false;

    }

}


