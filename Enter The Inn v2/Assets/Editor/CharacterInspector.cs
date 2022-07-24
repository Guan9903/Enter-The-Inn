using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterConfig))]
public class CharacterInspector : Editor
{
    private SerializedProperty type;
    private SerializedProperty id;
    private SerializedProperty characterName;
    private SerializedProperty intHealth;
    private SerializedProperty health;
    private SerializedProperty speed;


    private void OnEnable()
    {
        type = serializedObject.FindProperty("characterType");
        id = serializedObject.FindProperty("id");
        characterName = serializedObject.FindProperty("characterName");
        intHealth = serializedObject.FindProperty("intHealth");
        health = serializedObject.FindProperty("health");
        speed = serializedObject.FindProperty("speed");

    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(type);
        EditorGUILayout.PropertyField(id);
        EditorGUILayout.PropertyField(characterName);
        EditorGUILayout.PropertyField(speed);


        switch (type.enumValueIndex)
        {
            case 0:
                EditorGUILayout.PropertyField(intHealth);
                break;
            case 1:
                EditorGUILayout.PropertyField(health);
                break;
            case 2:
                break;
        }

        serializedObject.ApplyModifiedProperties();

    }
}
