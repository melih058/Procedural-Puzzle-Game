using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(LevelDataToJSON))]
public class LevelDataToJSONEditor : Editor
{
    private LevelDataToJSON _levelDataToJSON;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _levelDataToJSON = (LevelDataToJSON)target;

        if (GUILayout.Button("Convert to JSON"))
        {
            _levelDataToJSON.convertLevelDataSO();

        }
        if (_levelDataToJSON.levelDataSOToBeConverted == null)
        {
            EditorGUILayout.HelpBox("You must assign a LevelDataSO for convert", MessageType.Warning);
        }
       

    }
}
