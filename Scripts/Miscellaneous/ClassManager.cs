using UnityEngine;

public class ClassManager : MonoBehaviour
{
    public CameraLogic CameraLogic;
    public Player Player;
    public Movement Movement;
    public Dodge Dodge;
    public AttackLogic Attack;
    public StateMachine StateMachine;
    public LevelSystem LevelSystem;
    public CharacterStats CharacterStats;
    public Health Health;
    public Stamina Stamina;
    public DivinePoint DivinePoint;
    public SkillTreeManager SkillTreeManager;
    public RespawnManager RespawnManager;
    public PlayerBar PlayerBar;
    public Hotkeys Hotkeys;
    public AttackStateMachine AttackStateMachine;
    public UIManager UIManager;
    public AttackLogic AttackLogic;
    public LevelUpPopup LevelUpPopup;
    public XPPopupManager XPPopupManager;
    public DivinePointAnimations DivinePointAnimations;

    public static ClassManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
                Destroy(gameObject); 
        }
    }

    void Start()
    {
        CameraLogic = FindObjectOfType<CameraLogic>();
        Player = FindObjectOfType<Player>();
        Movement = FindObjectOfType<Movement>();
        Dodge = FindObjectOfType<Dodge>();
        Attack = FindObjectOfType<AttackLogic>();
        StateMachine = FindObjectOfType<StateMachine>();
        LevelSystem = FindObjectOfType<LevelSystem>();
        CharacterStats = FindObjectOfType<CharacterStats>();
        Health = FindObjectOfType<Health>();
        Stamina = FindObjectOfType<Stamina>();
        DivinePoint = FindObjectOfType<DivinePoint>();
        SkillTreeManager = FindObjectOfType<SkillTreeManager>();
        RespawnManager = FindObjectOfType<RespawnManager>();
        PlayerBar = FindObjectOfType<PlayerBar>();
        Hotkeys = FindObjectOfType<Hotkeys>();
        AttackStateMachine = FindObjectOfType<AttackStateMachine>();
        UIManager = FindObjectOfType<UIManager>();
        AttackLogic = FindObjectOfType<AttackLogic>();
        LevelUpPopup = FindObjectOfType<LevelUpPopup>();
        XPPopupManager = FindObjectOfType<XPPopupManager>();
        DivinePointAnimations = FindObjectOfType<DivinePointAnimations>();
    }
}
