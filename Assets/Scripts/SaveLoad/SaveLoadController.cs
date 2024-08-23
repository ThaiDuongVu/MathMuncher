using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadController : MonoBehaviour
{
    #region Singleton

    private static SaveLoadController _saveLoadControllerInstance;

    public static SaveLoadController Instance
    {
        get
        {
            if (_saveLoadControllerInstance == null) _saveLoadControllerInstance = FindFirstObjectByType<SaveLoadController>();
            return _saveLoadControllerInstance;
        }
    }

    #endregion

    private const string TempSaveFileName = "mathTemp.save";
    private const string PermaSaveFileName = "mathPerma.save";
    private const string SettingsSaveFileName = "mathSettings.save";
    private string _tempSaveFilePath;
    private string _permaSaveFilePath;
    private string _settingsSaveFilePath;

    private FileStream _fileStream;
    private readonly BinaryFormatter _binaryFormatter = new();

    #region Unity Events

    private void Awake()
    {
        _tempSaveFilePath = $"{Application.persistentDataPath}/{TempSaveFileName}";
        _permaSaveFilePath = $"{Application.persistentDataPath}/{PermaSaveFileName}";
        _settingsSaveFilePath = $"{Application.persistentDataPath}/{SettingsSaveFileName}";
    }

    #endregion

    #region Temp Save & Load

    public void SaveTemp(TempSaveData saveData)
    {
        // Save current data in the predetermined path
        _fileStream = new FileStream(_tempSaveFilePath, FileMode.Create);
        _binaryFormatter.Serialize(_fileStream, saveData);
        _fileStream.Close();
    }

    public TempSaveData LoadTemp()
    {
        // If no file detected then return default state
        if (!File.Exists(_tempSaveFilePath))
        {
            return new();
        }

        // Return save data from file
        _fileStream = new FileStream(_tempSaveFilePath, FileMode.Open);
        var data = _binaryFormatter.Deserialize(_fileStream) as TempSaveData;
        _fileStream.Close();
        return data;
    }

    public void ResetTemp()
    {
        SaveTemp(new());
    }

    #endregion

    #region Perma Save & Load

    public void SavePerma(PermaSaveData saveData)
    {
        // Save current data in the predetermined path
        _fileStream = new FileStream(_permaSaveFilePath, FileMode.Create);
        _binaryFormatter.Serialize(_fileStream, saveData);
        _fileStream.Close();
    }

    public PermaSaveData LoadPerma()
    {
        // If no file detected then return default state
        if (!File.Exists(_permaSaveFilePath))
        {
            return new();
        }

        // Return save data from file
        _fileStream = new FileStream(_permaSaveFilePath, FileMode.Open);
        var data = _binaryFormatter.Deserialize(_fileStream) as PermaSaveData;
        _fileStream.Close();
        return data;
    }

    #endregion

    #region Settings Save & Load

    public void SaveSettings(SettingsSaveData saveData)
    {
        // Save current data in the predetermined path
        _fileStream = new FileStream(_settingsSaveFilePath, FileMode.Create);
        _binaryFormatter.Serialize(_fileStream, saveData);
        _fileStream.Close();
    }

    public SettingsSaveData LoadSettings()
    {
        // If no file detected then return default state
        if (!File.Exists(_settingsSaveFilePath))
        {
            return new();
        }

        // Return save data from file
        _fileStream = new FileStream(_settingsSaveFilePath, FileMode.Open);
        var data = _binaryFormatter.Deserialize(_fileStream) as SettingsSaveData;
        _fileStream.Close();
        return data;
    }

    #endregion
}
