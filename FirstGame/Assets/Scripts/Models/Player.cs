using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum ScoreType
{
    COIN, DIAMOND, POWER
}

public class Player : MonoBehaviour
{
    /*-------------PLAYER DATA-------------*/
    public int _currentSkinId;

    public int _currentWalkingId;

    public int _currentRunningId;

    public int _currentCoin;

    public int _currentDiamond;

    public string userName;

    public bool isGuest;

    public int _currentKeyCount;

    /*-------------PLAYER SETTINGS DATA-------------*/

    public float _musicVolumeSettings;

    public float _soundFXVolumeSettings;

    public int _qualitySettingsIndex;

    public int _joystickSettings;

    public bool _isNotificationOn;

    public bool _isLiked;

    public bool _isRated;

    /*----------------PERFORMANCE DATA---------*/
    public int _highPunchPoint;

    public float _finishPunchPower;

    public int _level;


    /*-------------SHOPPING DATA-------------*/
    public List<int> _purchasedWalkingAnims;

    public List<int> _purchasedRunningAnims;

    public List<int> _purchasedCharacterSkin;

    public void initData(UserData data)
    {
        _currentSkinId = data._currentSkinId;
        _currentWalkingId = data._currentGloveId;
        _currentRunningId = data._currentRunningId;
        _currentCoin = data._currentCoin;
        _currentDiamond = data._currentDiamond;
        _currentKeyCount = data._currentKeyCount;
        userName = data.userName;
        _musicVolumeSettings = data._musicVolumeSettings;
        _soundFXVolumeSettings = data._soundFXVolumeSettings;
        _isNotificationOn = data._isNotificationOn;
        _qualitySettingsIndex = data._qualitySettingsIndex;
        _joystickSettings = data._joystickSettings;
        isGuest = data.isGuest;
        _highPunchPoint = data._highPunchPoint;
        _finishPunchPower = data._finishPunchPower;
        _level = data._level;
        _purchasedWalkingAnims = data._purchasedGloves.ToList();
        _purchasedCharacterSkin = data._purchasedSkins.ToList();
        _purchasedRunningAnims = data._unlockedGloves.ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
