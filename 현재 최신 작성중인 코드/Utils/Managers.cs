using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Managers : MonoBehaviour 
{
    static Managers s_instance; // 유일성이 보장된다
    public static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다

    #region InGame Contents SingleTon
    //GameManager _game;
    //EffectManager _effectManager;
    //BuffManager _buffManager;
    //AIManager _aIManager;
    //public static GameManager Game { get => GetManager(Instance._game); set { Instance._game =value; } }

    //public static EffectManager effectManager { get => GetManager(Instance._effectManager); set { Instance._effectManager = value; } }
    //public static BuffManager buffManager { get => GetManager(Instance._buffManager); set { Instance._buffManager = value; } }
    //public static AIManager aIManager{ get => GetManager(Instance._aIManager); set { Instance._aIManager = value; } }


    #endregion

    #region SingleTon Core Don't Destory
    //PoolManager _pool = new PoolManager();
    //ResourceManager _resource = new ResourceManager();
    //SoundManager _sound = new SoundManager();
    //UIManager _ui = new UIManager();
    //[SerializeField] SceneManagerEx _scene;

    //public static PoolManager Pool => Instance._pool;
    //public static ResourceManager Resource => Instance._resource;
    //public static SceneManagerEx Scene => Instance._scene;
    //public static SoundManager Sound { get { return Instance._sound; } }
    //public static UIManager UI { get { return Instance._ui; } }
    //public static PhotonManager PhotonManager => Instance._photonManager;


    #endregion

    #region SingleTon ScriptableObject
    private const string SettingFileDirectory = "Assets/Resources";
    //private const string SettingFilePath = "Assets/Resources/Setting/GameSettings.asset";//정확한 파일 위치,만약파
    //[SerializeField] ProductSetting _productSetting;
    //[SerializeField] GameSetting _gameSetting;
    //[SerializeField] UISetting _uISetting;
    //[SerializeField] AISetting _aISetting;


    //public static ProductSetting ProductSetting => GetSingleScriptableOjbectr(Instance._productSetting); 
    //public static GameSetting GameSetting => GetSingleScriptableOjbectr(Instance._gameSetting);
    //public static AISetting AISetting => GetSingleScriptableOjbectr(Instance._aISetting);
    #endregion

    void Start()
    {
#if UNITY_ANDROID

#endif
        Application.targetFrameRate = 30;
        Screen.SetResolution(1920, 1080, true);
        Init();

	}
    static T GetManager<T>(T go) where T : Component
    {
        if(go == null)
        {
            go = FindObjectOfType<T>();
        }

        return go;
    }

  
    static void Init()
    {
        if (s_instance == null)
        {
			GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            if (Application.isPlaying == false)
            {
                s_instance = go.GetComponent<Managers>();
                return;
            }
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
            //s_instance._pool.Init();
            //s_instance._input.Init();
        }
    }


    //코어 클리어
    public static void Clear()
    {
        //Input.Clear();
        //UI.Clear();
        //Pool.Clear();

    }


    public static void ContentsInit()
    {

    }
}
