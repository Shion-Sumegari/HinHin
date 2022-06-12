using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UserDataEditor : EditorWindow
{
    [MenuItem("UserData/UserDataEditor")]
    public static void OpenWindow()
    {
        UserDataEditor window = GetWindow<UserDataEditor>("User data editor");
        
    }
    public UserData userData;
    int gloveProgess;
    private void OnEnable()
    {
        userData = GameUtils.LoadPlayerData();
        gloveProgess = PlayerPrefs.GetInt(GameConfigs.UNLOCK_ITEM_PROCESS_KEY);
    }
    private void OnGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            DrawSetting(userData);
            if (check.changed)
            {
                GameUtils.SavePlayerData(userData);
                PlayerPrefs.SetInt(GameConfigs.UNLOCK_ITEM_PROCESS_KEY, gloveProgess);
            }
        }
        
        if (GUILayout.Button("Save data"))
        {
            GameUtils.SavePlayerData(userData);
        }
        
        if(GUILayout.Button("Set Glove Progress"))
        {
            PlayerPrefs.SetInt(GameConfigs.UNLOCK_ITEM_PROCESS_KEY, gloveProgess);
        }
        //DrawProperties(serialized.FindProperty("Data"), true);
    }
    void DrawSetting(UserData userData)
    {
        //EditorGUILayout.BeginHorizontal();
        //GUILayout.Label("Level");
        //userData._level = EditorGUILayout.IntField(userData._level);
        //EditorGUILayout.EndHorizontal();

        //for(int i = 0; i < userData._unlockedGloves.Length; i++)
        //{
        //    userData._unlockedGloves[i] = EditorGUILayout.PropertyField(userData._unlockedGloves[i]);
        //}

        //EditorGUILayout.BeginHorizontal();
        //GUILayout.Label("glove progress");
        //gloveProgess = EditorGUILayout.IntField(gloveProgess);
        //EditorGUILayout.EndHorizontal();
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty dataProperty = so.FindProperty("userData");
        EditorGUILayout.PropertyField(dataProperty, true);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("glove progress");
        gloveProgess = EditorGUILayout.IntField(gloveProgess);
        EditorGUILayout.EndHorizontal();
        so.ApplyModifiedProperties();
    }
    protected void DrawProperties(SerializedProperty prop, bool drawChildren)
    {
        string lastPropPath = string.Empty;
        foreach (SerializedProperty p in prop)
        {
            if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                EditorGUILayout.EndHorizontal();
                if (p.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    DrawProperties(p, drawChildren);
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath)) continue;
                lastPropPath = p.propertyPath;
                EditorGUILayout.PropertyField(p, drawChildren);
            }
        }
    }
}
