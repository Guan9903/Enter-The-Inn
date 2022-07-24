using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using TMPro.EditorUtilities;

[CustomEditor(typeof(WeaponConfig))]
public class WeaponInspector : Editor
{
    private SerializedProperty type;
    private SerializedProperty id;
    private SerializedProperty weaponName;
    private SerializedProperty weaponIcon;
    private SerializedProperty weaponCursor;
    private SerializedProperty weaponDamage;
    private SerializedProperty ammoObj;
    private SerializedProperty ammoIcon;
    private SerializedProperty ammoNums;
    private SerializedProperty magazineSize;
    private SerializedProperty reloadTime;
    private SerializedProperty ammoSpeed;
    private SerializedProperty fireRange;
    private SerializedProperty fireRate;
    private SerializedProperty weaponDurability;

    private void OnEnable()
    {
        type = serializedObject.FindProperty("weaponType");
        id = serializedObject.FindProperty("id");
        weaponName = serializedObject.FindProperty("weaponName");
        weaponIcon = serializedObject.FindProperty("weaponIcon");
        weaponCursor = serializedObject.FindProperty("weaponCursor");
        weaponDamage = serializedObject.FindProperty("weaponDamage");
        ammoObj = serializedObject.FindProperty("ammoObj");
        ammoIcon = serializedObject.FindProperty("ammoIcon");
        ammoNums = serializedObject.FindProperty("ammoNums");
        magazineSize = serializedObject.FindProperty("magazineSize");
        reloadTime = serializedObject.FindProperty("reloadTime");
        ammoSpeed = serializedObject.FindProperty("ammoSpeed");   
        fireRange = serializedObject.FindProperty("fireRange");
        fireRate = serializedObject.FindProperty("fireRate");
        weaponDurability = serializedObject.FindProperty("weaponDurability");

    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(type);
        EditorGUILayout.PropertyField(id);
        EditorGUILayout.PropertyField(weaponName);
        EditorGUILayout.PropertyField(weaponIcon);
        EditorGUILayout.PropertyField(weaponCursor);
        EditorGUILayout.PropertyField(weaponDamage);

        switch (type.enumValueIndex)
        {
            case 0:
                EditorGUILayout.PropertyField(ammoObj);
                EditorGUILayout.PropertyField(ammoIcon);
                EditorGUILayout.PropertyField(ammoNums);
                EditorGUILayout.PropertyField(magazineSize);
                EditorGUILayout.PropertyField(reloadTime);
                EditorGUILayout.PropertyField(ammoSpeed);
                EditorGUILayout.PropertyField(fireRange);
                EditorGUILayout.PropertyField(fireRate);
                break;
            case 1:
                EditorGUILayout.PropertyField(ammoObj);
                EditorGUILayout.PropertyField(ammoIcon);
                EditorGUILayout.PropertyField(ammoNums);
                EditorGUILayout.PropertyField(magazineSize);
                EditorGUILayout.PropertyField(reloadTime);
                EditorGUILayout.PropertyField(ammoSpeed);
                EditorGUILayout.PropertyField(fireRange);
                EditorGUILayout.PropertyField(fireRate);
                break;
            case 2:
                EditorGUILayout.PropertyField(weaponDurability);
                break;
        }

        serializedObject.ApplyModifiedProperties();

    }
}
