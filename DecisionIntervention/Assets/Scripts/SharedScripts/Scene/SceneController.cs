using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;
using RoboRyanTron.Unite2017.Events;

public class SceneController : MonoBehaviour {

	private int curskybox;
	public int currentSkybox{
		get{ return curskybox;}
		set{ curskybox = value;} 

	}
	public Material[] skyboxes;
	private GameObject stressMenu;

	public static SceneController Instance
	{ get { return instance; } }

	private static SceneController instance = null;

	public static bool isPlayingTimeLine;
	//public static bool isSceneLoading = false;

	//private Transform player;

	private Animator anim = null;

	private Canvas sceneCanvas;

	[SerializeField]private GameObject loadingSceneActivator;

    [SerializeField] private List<string> aSyncedScenes;

	[Header("Events")]
	//[SerializeField]private UnityEvent onGameInitialized;
    [SerializeField]private UnityEvent onSceneChange;
    [SerializeField] private UnityEvent onSceneAdded;

    [Header("Event_Invoke")]
    [SerializeField] private GameEvent onStartChangeScene;
    [SerializeField] private GameEvent onEndChangeScene;

    [Header("References")]
    [SerializeField] private DataManager DATA_MANAGER;

   // [SerializeField]private BoolVariable isSceneLoading;

	void Awake()
	{
        DATA_MANAGER.currentSceneName.name = SceneManager.GetActiveScene().name;

        RenderSettings.skybox = skyboxes [currentSkybox];


		if (instance) {
			Debug.LogWarning ("There are two instances of scene controller - deleting late instance.");
			DestroyImmediate (gameObject);

			return;
		}
		instance = this; 

		DontDestroyOnLoad (gameObject);


		originalGazeTime = GazeInputModule.GazeTimeInSeconds;

	}

	float originalGazeTime;
	IEnumerator Start(){

		SceneManager.activeSceneChanged += OnActiveSceneChange;
       // SceneManager.sceneLoaded += OnSceneLoaded;

		anim = GetComponentInChildren<Animator>();

		//anim.SetTrigger ("FadeOut");

		yield return null;

		StartCoroutine(TakeOffFade());
	}



	IEnumerator TakeOffFade(){

		anim.SetTrigger ("FadeOut");

		while (true) {
			yield return null;

			if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Clear")) {

                //GazeInputModule.GazeTimeInSeconds = originalGazeTime;
                DATA_MANAGER.isSceneLoading.isOn = false;
				
				//isSceneLoading.isOn = false;
				yield break;

			}
		}

	}

    public void SetActiveScene(string scene)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));

    }
	
    public void Load_ASync(string scene)
    {
        StartCoroutine(LoadAsyncAdditive(scene));
    }
    public IEnumerator LoadAsyncAdditive(string scene)
    {
   
       aSyncedScenes.Add(scene);
       
      yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);//, LoadSceneMode.Additive);
       

        //  DATA_MANAGER.isSceneLoading.isOn = true;


        onSceneAdded.Invoke();

      // yield return new WaitWhile(() => !async.isDone);

        //DATA_MANAGER.isSceneLoading.isOn = false;

    }

    public void UnLoadScene_Async(string scene)
    {
        StartCoroutine(UnLoadAsync(scene));
    }

    public IEnumerator UnLoadAsync(string scene)
    {
     
        yield return SceneManager.UnloadSceneAsync(scene);
        Resources.UnloadUnusedAssets();

    }
    AsyncOperation async = null;


    public void Load(string scene)
    {
        //	GazeInputModule.GazeTimeInSeconds = Mathf.Infinity;

        if (!DATA_MANAGER.isSceneLoading.isOn)
        {
            DATA_MANAGER.isSceneLoading.isOn = true;
            //	GazeInputModule.GazeTimeInSeconds = Mathf.Infinity;
        }
        else
        {
            Debug.LogWarning("There is more that one scene attempting to load.");
            //return;
        }

        onStartChangeScene.Raise();
        

        onSceneChange.Invoke();

       

        StartCoroutine(ChangeScene(scene));


    }
    // AsyncOperation async = null;
    public IEnumerator ChangeScene (string scene){

        anim.SetTrigger("FadeIn");

        while (true) {

			yield return null;

			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Faded")) {

				StartCoroutine (WhileSceneIsLoading (scene));

				yield break;

			}
		}


	}//this plays before OnlevelLoad()...

    //  public Text changeSceneText;
    public GameObject sceneTransitionParent;
	IEnumerator WhileSceneIsLoading(string scene){

        sceneTransitionParent.gameObject.SetActive(true);
        //GetComponentInChildren<Text>(true);

		RawImage Rimage = sceneTransitionParent.GetComponentInChildren<RawImage> (true);
		Vector2 RiSize = Rimage.rectTransform.sizeDelta;
		//Rimage.gameObject.SetActive (true);

		//if(loadingSceneActivator)
		//	loadingSceneActivator.SetActive (true);

		Rimage.rectTransform.sizeDelta = new Vector2 (0, 50);

		async = SceneManager.LoadSceneAsync (scene, LoadSceneMode.Additive);
		async.allowSceneActivation = false;

        async.priority = 1;

        //image.color = Color.red;//new Color(image.color.r, image.color.g, image.color.b, Mathf.PingPong(Time.time,1));
        while (async.progress < 0.9f) {

            Debug.Log("ASYNC PROGRESS AND DONE:" + async.progress + async.isDone);
			Rimage.rectTransform.sizeDelta = new Vector2 (0.5f * (500 * async.progress), Rimage.rectTransform.sizeDelta.y);
			yield return new WaitForSecondsRealtime (0.05f);

		}
		async.allowSceneActivation = true;
		float perc = 0.5f;
		while(!async.isDone)
		{
			yield return null;
			perc = Mathf.Lerp(perc, 1f, 0.05f);
			Rimage.rectTransform.sizeDelta = new Vector2 (perc * (500 * async.progress), Rimage.rectTransform.sizeDelta.y);
		}

        DATA_MANAGER.isSceneLoading.isOn = false;

        sceneTransitionParent.SetActive(false);
       
     

        StartCoroutine(TakeOffFade());

        onEndChangeScene.Raise();
    }
    //void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    //{
    //    Camera.main.GetComponent<AudioListener>().enabled = false;
        
    //}
    void OnActiveSceneChange(Scene oldActive, Scene newActive){

      StartCoroutine(SetSettingsAfterLoad(newActive));

        //RenderSettings.skybox = skyboxes [currentSkybox];

        //if(OrientationAdjustment.Instance != null)
        //	OrientationAdjustment.Instance.OrientationChangeToGlobalFront ();

        //StartCoroutine(TakeOffFade());

    }
    IEnumerator SetSettingsAfterLoad(Scene scene)
    {
        yield return new WaitUntil(() => scene.isLoaded);// !DATA_MANAGER.isSceneLoading && async.isDone);
        sceneCanvas = GetComponentInChildren<Canvas>();
        if (sceneCanvas.worldCamera == null) ;
        sceneCanvas.worldCamera = Camera.main;

    }

    public void ResetCurrentGame(){

		Load (SceneManager.GetActiveScene ().name);

	}

	public void ResetGame (string scene){

		Load (scene);

	}
	public void ChangeSkyBox () {



		if (currentSkybox < skyboxes.Length) {
			++currentSkybox;
		} else {
			currentSkybox = 0;
		}
		RenderSettings.skybox = skyboxes [currentSkybox];
	}

	public string GetCurrentSceneName(){

		return SceneManager.GetActiveScene ().name;

	}






}
