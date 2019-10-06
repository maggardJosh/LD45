using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Custom/Game Setting")]
public class GameSettings : ScriptableObject
{
    public float tileSize = 16;
    public float collisionOffsetValue = .1f;
    public float gravity = 1f;
    public float minYVel = -5f;
    public GameObject pickupPrefab;
    public GameObject interactIndicatorPrefab;
    public SkeletonSettingGrouping ghostSetting;
    public Color bgColor;
    public GameObject essentialsPrefab;

    public float loadSceneFadeTime = 1f;
    public AnimationCurve loadInCurve;
    public AnimationCurve loadOutCurve;

    public float randomSFXAmount = .1f;

    public GameObject groundHitParticlePrefab;

    [Header("Audio")]
    public AudioClip victorySFX;
    public AudioClip jumpSFX;
    public AudioClip pickupPartSFX;
    public AudioClip discardPartSFX;
    public AudioClip failSFX;
    public AudioClip doorOpenSFX;
    public AudioClip hitGroundSFX;
    public AudioClip menuBlipSFX;
    public AudioClip resetSFX;

    private static GameSettings _instance;
    private static GameSettings Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<GameSettings>("GameSettings");
            return _instance;
        }
    }
    public static float TileSize { get => Instance.tileSize; }
    public static float CollisionOffsetValue { get => Instance.collisionOffsetValue; }
    public static float Gravity { get => Instance.gravity; }
    public static float MinYVel { get => Instance.minYVel; }
    public static GameObject PickupPrefab { get => Instance.pickupPrefab; }
    public static GameObject InteractIndicatorPrefab { get => Instance.interactIndicatorPrefab; }
    public static SkeletonSettingGrouping GhostSetting { get => Instance.ghostSetting; }
    public static Color BGColor { get => Instance.bgColor; }
    public static GameObject EssentialsPrefab { get => Instance.essentialsPrefab; }

    public static float LoadSceneFadeTime { get => Instance.loadSceneFadeTime; }
    public static AnimationCurve LoadInCurve { get => Instance.loadInCurve; }
    public static AnimationCurve LoadOutCurve { get => Instance.loadOutCurve; }

    public static AudioClip VictorySFX { get => Instance.victorySFX; }
    public static AudioClip JumpSFX { get => Instance.jumpSFX; }
    public static AudioClip PickupPartSFX { get => Instance.pickupPartSFX; }
    public static AudioClip DiscardPartSFX { get => Instance.discardPartSFX; }
    public static AudioClip FailSFX { get => Instance.failSFX; }
    public static AudioClip DoorOpenSFX { get => Instance.doorOpenSFX; }
    public static AudioClip HitGroundSFX { get => Instance.hitGroundSFX; }
    public static AudioClip MenuBlipSFX { get => Instance.menuBlipSFX; }
    public static AudioClip ResetSFX { get => Instance.resetSFX; }

    public static GameObject GroundHitParticlePrefab { get => Instance.groundHitParticlePrefab; }

    public static float RandomSFXAmount { get => Instance.randomSFXAmount; }

    public static bool LevelCompleted(string sceneName)
    {
        return PlayerPrefs.GetInt("levelcompleted_" + sceneName, 0) == 1;
    }

    public static void SetLevelCompleted(string sceneName)
    {
        PlayerPrefs.SetInt("levelcompleted_" + sceneName, 1);
    }
}