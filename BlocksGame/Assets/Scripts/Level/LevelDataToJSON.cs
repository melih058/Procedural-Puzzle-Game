using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelDataToJSON : MonoBehaviour
{
    [SerializeField] private LevelDataSO _levelDataSOToBeConverted;


    public void convertLevelDataSO()
    {
        if (_levelDataSOToBeConverted == null)
        {
            Debug.LogWarning("There isn't file for convert. Please!! assing a LevelDataSO.");
            return;
        }
        string json = JsonUtility.ToJson(_levelDataSOToBeConverted);
        string targetPath = Application.dataPath + "/SavedLevels/" + _levelDataSOToBeConverted.name + ".json";
        File.WriteAllText(targetPath, json);
        Debug.Log("File converted to JSON -> " + targetPath);
        _levelDataSOToBeConverted = null;
    }

    public LevelDataSO levelDataSOToBeConverted => _levelDataSOToBeConverted;
}
