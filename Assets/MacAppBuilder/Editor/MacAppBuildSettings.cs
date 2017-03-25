using UnityEngine;
using UnityEditor;
using System.Collections;

/**
 * MacAppBuildSettings.cs
 * Author: Luke Holland (http://lukeholland.me/)
 */

public class MacAppBuildSettings : ScriptableObject
{

	public string bundleIdentifier = "";
	public string applicationCategory = "public.app-category.games";
	public string applicationDescription = "[YEAR] [YOUR COMPANY NAME]. All rights reserved.";

	public string provisioningProfileApplication = "3rd Party Mac Developer Application: [YOUR COMPANY NAME]";
	public string provisioningProfileInstaller = "3rd Party Mac Developer Installer: [YOUR COMPANY NAME]";

	public Object playerIconsFile;
	public Object entitlementsFile;

	public string playerIconsPath {
		get {
			if(playerIconsFile==null) return null;
			else {
				return AssetDatabase.GetAssetPath(playerIconsFile);
			}
		}
	}

	public string entitlementsPath {
		get {
			if(entitlementsFile==null) return null;
			else {
				return AssetDatabase.GetAssetPath(entitlementsFile);
			}
		}
	}

}

[CustomEditor(typeof(MacAppBuildSettings))]
public class MacAppBuildSettingsEditor : Editor
{

	private SerializedProperty _propertyBundleIdentifier;
	private SerializedProperty _propertyApplicationCategory;
	private SerializedProperty _propertyApplicationDescription;
	private SerializedProperty _propertyProvisioningProfileApplication;
	private SerializedProperty _propertyProvisioningProfileInstaller;
	private SerializedProperty _propertyFilePlayerIcons;
	private SerializedProperty _propertyFileEntitlements;

	public void OnEnable()
	{
		_propertyBundleIdentifier = serializedObject.FindProperty("bundleIdentifier");
		_propertyApplicationCategory = serializedObject.FindProperty("applicationCategory");
		_propertyApplicationDescription = serializedObject.FindProperty("applicationDescription");
		_propertyProvisioningProfileApplication = serializedObject.FindProperty("provisioningProfileApplication");
		_propertyProvisioningProfileInstaller = serializedObject.FindProperty("provisioningProfileInstaller");
		_propertyFilePlayerIcons = serializedObject.FindProperty("playerIconsFile");
		_propertyFileEntitlements = serializedObject.FindProperty("entitlementsFile");
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update();

		EditorGUILayout.LabelField("Application Settings",EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(_propertyBundleIdentifier,new GUIContent("Bundle Indentifier"));
		EditorGUILayout.PropertyField(_propertyApplicationCategory,new GUIContent("Category"));
		EditorGUILayout.PropertyField(_propertyApplicationDescription,new GUIContent("Description"));

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Provisioning Profiles",EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(_propertyProvisioningProfileApplication,new GUIContent("Application"));
		EditorGUILayout.PropertyField(_propertyProvisioningProfileInstaller,new GUIContent("Installer"));

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Required Files",EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(_propertyFilePlayerIcons,new GUIContent("Player Icons"));
		EditorGUILayout.PropertyField(_propertyFileEntitlements,new GUIContent("Entitlements"));


		serializedObject.ApplyModifiedProperties();
	}

}
