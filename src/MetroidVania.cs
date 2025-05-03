using Modding;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UObject = UnityEngine.Object;
using System.IO;
using SFCore.Utils;

namespace MetroidVania;

public class MetroidVania : Mod
{
    private const bool Debug = true;
    private const string AbPath = "C:\\Users\\SFG\\Documents\\Projects\\Unity Projects\\TorvusBog Assets\\Assets\\AssetBundles\\";
    internal static MetroidVania Instance;

    public AssetBundle AbScene { get; private set; } = null;
    public AssetBundle AbMat { get; private set; } = null;

    public override string GetVersion() => Util.GetVersion(Assembly.GetExecutingAssembly());

    //public override List<ValueTuple<string, string>> GetPreloadNames()
    //{
    //    return new List<ValueTuple<string, string>>
    //    {
    //        new ValueTuple<string, string>("White_Palace_18", "White Palace Fly"),
    //        new ValueTuple<string, string>("White_Palace_18", "saw_collection/wp_saw"),
    //        new ValueTuple<string, string>("White_Palace_18", "saw_collection/wp_saw (2)"),
    //        new ValueTuple<string, string>("White_Palace_18", "Soul Totem white_Infinte"),
    //        new ValueTuple<string, string>("White_Palace_18", "Area Title Controller"),
    //        new ValueTuple<string, string>("White_Palace_18", "glow response lore 1/Glow Response Object (11)"),
    //        new ValueTuple<string, string>("White_Palace_18", "_SceneManager"),
    //        new ValueTuple<string, string>("White_Palace_18", "Inspect Region"),
    //        new ValueTuple<string, string>("White_Palace_18", "_Managers/PlayMaker Unity 2D"),
    //        new ValueTuple<string, string>("White_Palace_18", "Music Region (1)"),
    //        new ValueTuple<string, string>("White_Palace_17", "WP Lever"),
    //        new ValueTuple<string, string>("White_Palace_17", "White_ Spikes"),
    //        new ValueTuple<string, string>("White_Palace_17", "Cave Spikes Invis"),
    //        new ValueTuple<string, string>("White_Palace_09", "Quake Floor"),
    //        new ValueTuple<string, string>("Grimm_Divine", "Charm Holder"),
    //        new ValueTuple<string, string>("White_Palace_03_hub", "WhiteBench"),
    //        new ValueTuple<string, string>("Crossroads_07", "Breakable Wall_Silhouette"),
    //        new ValueTuple<string, string>("Deepnest_East_Hornet_boss", "Hornet Outskirts Battle Encounter"),
    //        new ValueTuple<string, string>("White_Palace_03_hub", "door1"),
    //        new ValueTuple<string, string>("White_Palace_03_hub", "Dream Entry")
    //    };
    //}

    public MetroidVania() : base("MetroidVania")
    {
        Instance = this;

        //AchievementHelper.Initialize();
        //AchievementHelper.AddAchievement(AchievementStrings.DefeatedWeaverPrincess_Key, MetroidVania.GetSprite(TextureStrings.AchievementWeaverPrincessKey), LanguageStrings.AchievementDefeatedWeaverPrincessTitleKey, LanguageStrings.AchievementDefeatedWeaverPrincessTextKey, true);

        InitInventory();

        InitCallbacks();
    }

    public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        Log("Initializing");
        Instance = this;

        LoadAssetbundles();

        GameManager.instance.StartCoroutine(Register2BossModCore());

        Log("Initialized");
    }

    private void LoadAssetbundles()
    {
#pragma warning disable CS0162 // Unerreichbarer Code wurde entdeckt.
        Log("Loading AssetBundles");
        Assembly asm = Assembly.GetExecutingAssembly();
        if (AbScene == null)
        {
            if (!Debug)
            {
                using (Stream s = asm.GetManifestResourceStream("MetroidVania.Resources.tbscenes"))
                {
                    if (s != null)
                    {
                        AbScene = AssetBundle.LoadFromStream(s);
                    }
                }
            }
            else
            {
                AbScene = AssetBundle.LoadFromFile(AbPath + "tbscenes");
            }
        }
        if (AbMat == null)
        {
            if (!Debug)
            {
                using (Stream s = asm.GetManifestResourceStream("MetroidVania.Resources.tbassets"))
                {
                    if (s != null)
                    {
                        AbMat = AssetBundle.LoadFromStream(s);
                    }
                }
            }
            else
            {
                AbMat = AssetBundle.LoadFromFile(AbPath + "tbassets");
            }
        }
        Log("Finished loading AssetBundles");
#pragma warning restore CS0162 // Unerreichbarer Code wurde entdeckt.
    }

    private void InitCallbacks()
    {
        // Hooks
        //ModHooks.GetPlayerBoolHook += OnGetPlayerBoolHook;
        //ModHooks.SetPlayerBoolHook += OnSetPlayerBoolHook;
        //ModHooks.GetPlayerIntHook += OnGetPlayerIntHook;
        //ModHooks.SetPlayerIntHook += OnSetPlayerIntHook;
        ModHooks.ApplicationQuitHook += SaveGlobalSettings;
        //ModHooks.LanguageGetHook += OnLanguageGetHook;
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void InitInventory()
    {
        //ItemHelper.init();
        //ItemHelper.AddNormalItem(StateNames.InvStateHornet, MetroidVania.GetSprite(TextureStrings.InvHornetKey), nameof(_saveSettings.SFGrenadeTestOfTeamworkHornetCompanion), LanguageStrings.HornetInvNameKey, LanguageStrings.HornetInvDescKey);
    }

    private void OnSceneChanged(UnityEngine.SceneManagement.Scene from, UnityEngine.SceneManagement.Scene to)
    {
        string scene = to.name;

        if (scene == "Town")
        {
            var rGo = to.Find("right1");
            var tp = rGo.GetComponent<TransitionPoint>();
            tp.targetScene = "SF_MP2_TB01";
            tp.entryPoint = "sf_tb_left1";
        }
        //if (true)
        //{
        //    GameManager.instance.RefreshTilemapInfo(scene);
        //}
    }

    #region Get/Set Hooks

    //private string OnLanguageGetHook(string key, string sheet)
    //{
    //    if (LangStrings.ContainsKey(key, sheet))
    //    {
    //        return LangStrings.Get(key, sheet);
    //    }
    //    return Language.Language.GetInternal(key, sheet);
    //}

    //private bool OnGetPlayerBoolHook(string target)
    //{
    //    var tmpField = _saveSettingsType.GetField(target);
    //    if (tmpField != null)
    //    {
    //        return (bool) tmpField.GetValue(_saveSettings);
    //    }
    //    return PlayerData.instance.GetBoolInternal(target);
    //}

    //private void OnSetPlayerBoolHook(string target, bool val)
    //{
    //    var tmpField = _saveSettingsType.GetField(target);
    //    if (tmpField != null)
    //    {
    //        tmpField.SetValue(_saveSettings, val);
    //        return;
    //    }
    //    PlayerData.instance.SetBoolInternal(target, val);
    //}

    //private int OnGetPlayerIntHook(string target)
    //{
    //    var tmpField = _saveSettingsType.GetField(target);
    //    if (tmpField != null)
    //    {
    //        return (int) tmpField.GetValue(_saveSettings);
    //    }
    //    return PlayerData.instance.GetIntInternal(target);
    //}

    //private void OnSetPlayerIntHook(string target, int val)
    //{
    //    var tmpField = _saveSettingsType.GetField(target);
    //    if (tmpField != null)
    //    {
    //        tmpField.SetValue(_saveSettings, val);
    //    }
    //    else
    //    {
    //        PlayerData.instance.SetIntInternal(target, val);
    //    }
    //}

    #endregion Get/Set Hooks

    #region BossModCore Registration

    enum Commands
    {
        NumBosses,
        StatueName,
        StatueDescription,
        CustomScene,
        ScenePrefabName,
        STATUE_GO
    }
    private bool _r2BmcTimeout;
    private bool _r2BmcSuccess;
    private static readonly string R2BmcBmc = "BossModCore";
    private static readonly string R2BmcCom = $"{R2BmcBmc} - ";
    private static readonly string R2BmcSetNum = $" - {Commands.NumBosses}";
    private static readonly string R2BmcSetStatName = $" - {Commands.StatueName} - ";
    private static readonly string R2BmcSetStatDesc = $" - {Commands.StatueDescription} - ";
    private static readonly string R2BmcSetCustomScene = $" - {Commands.CustomScene} - ";
    private static readonly string R2BmcSetCustomSceneName = $" - {Commands.ScenePrefabName} - ";
    private static readonly string R2BmcSetStatGo = $" - {Commands.STATUE_GO} - ";

    private IEnumerator Register2BossModCore()
    {
        PlayerData pd = PlayerData.instance;
        _r2BmcTimeout = false;

        GameManager.instance.StartCoroutine(RegisterTimeout());

        while (!_r2BmcTimeout)
        {
            _r2BmcSuccess = pd.GetBool(R2BmcBmc);
            if (_r2BmcSuccess)
            {
                _r2BmcTimeout = true;
            }
            yield return null;
        }

        if (!_r2BmcSuccess)
        {
            Log(R2BmcBmc + " not found!");
            yield break;
        }
        Log(R2BmcBmc + " is able to be registered to!");
        yield return null;

        pd.SetInt(R2BmcCom + GetType().Name + R2BmcSetNum, 1);
        pd.SetString(R2BmcCom + GetType().Name + R2BmcSetStatName + "0", "Boss Statue Name");
        pd.SetString(R2BmcCom + GetType().Name + R2BmcSetStatDesc + "0", "Boss Statue Description");
        pd.SetBool(R2BmcCom + GetType().Name + R2BmcSetCustomScene + "0", false);
        pd.SetString(R2BmcCom + GetType().Name + R2BmcSetCustomSceneName + "0", "GG_Hornet_2");
        pd.SetVariable(R2BmcCom + GetType().Name + R2BmcSetStatGo + "0", new GameObject("StatePrefabGO"));
    }

    private IEnumerator RegisterTimeout()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        _r2BmcTimeout = true;
        _r2BmcSuccess = false;
    }

    #endregion BossModCore Registration
}