using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public string _playerId;

    /*-------------PLAYER DATA-------------*/
    public int _currentSkinId;

    public int _currentGloveId;

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
    public int[] _purchasedGloves;

    public int[] _unlockedGloves;

    public int[] _purchasedSkins;

    public UserData(Player player)
    {
        _currentSkinId = player._currentSkinId;
        userName = player.userName;
        _currentGloveId = player._currentWalkingId;
        _currentRunningId = player._currentRunningId;
        _currentCoin = player._currentCoin;
        _currentDiamond = player._currentDiamond;
        _currentKeyCount = player._currentKeyCount;
        _musicVolumeSettings = player._musicVolumeSettings;
        _soundFXVolumeSettings = player._soundFXVolumeSettings;
        _isNotificationOn = player._isNotificationOn;
        _isLiked = player._isLiked;
        _isRated = player._isRated;
        _qualitySettingsIndex = player._qualitySettingsIndex;
        _joystickSettings = player._joystickSettings;
        isGuest = player.isGuest;
        _highPunchPoint = player._highPunchPoint;
        _finishPunchPower = player._finishPunchPower;
        _level = player._level;
        _purchasedGloves = player._purchasedWalkingAnims.ToArray();
        _unlockedGloves = player._purchasedRunningAnims.ToArray();
        _purchasedSkins = player._purchasedCharacterSkin.ToArray();
    }

    public UserData(UserData playerData)
    {
        _currentSkinId = playerData._currentSkinId;
        userName = playerData.userName;
        _currentGloveId = playerData._currentGloveId;
        _currentRunningId = playerData._currentRunningId;
        _currentCoin = playerData._currentCoin;
        _currentDiamond = playerData._currentDiamond;
        _currentKeyCount = playerData._currentKeyCount;
        _musicVolumeSettings = playerData._musicVolumeSettings;
        _soundFXVolumeSettings = playerData._soundFXVolumeSettings;
        _isNotificationOn = playerData._isNotificationOn;
        _isLiked = playerData._isLiked;
        _isRated = playerData._isRated;
        _qualitySettingsIndex = playerData._qualitySettingsIndex;
        _joystickSettings = playerData._joystickSettings;
        isGuest = playerData.isGuest;
        _highPunchPoint = playerData._highPunchPoint;
        _finishPunchPower = playerData._finishPunchPower;
        _level = playerData._level;
        _purchasedGloves = playerData._purchasedGloves;
        _unlockedGloves = playerData._unlockedGloves;
        _purchasedSkins = playerData._purchasedSkins;
    }

    public UserData()
    {
        _playerId = "guest-01";
        userName = "";
        _currentSkinId = 1101;
        _currentGloveId = 1001;
        _currentRunningId = 2;
        _currentCoin = 0;
        _currentDiamond = 0;
        _currentKeyCount = 0;
        _musicVolumeSettings = 1f;
        _soundFXVolumeSettings = 1f;
        _isNotificationOn = true;
        _qualitySettingsIndex = 2;
        _joystickSettings = 1;
        isGuest = true;
        _isLiked = false;
        _isRated = false;
        _highPunchPoint = 0;
        _finishPunchPower = GameConfigs.MIN_FINISH_POWER;
        _level = 0;
        _purchasedGloves = new int[2] { 1001, 1002 };
        _unlockedGloves = new int[3] { 1001, 1002, 1003 };
        _purchasedSkins = new int[2] { 1101, 1102 };
    }

    public string ToString()
    {
        return "player id: " + _playerId + "\n" + "Player current skin ID: " + _currentSkinId;
    }
}
