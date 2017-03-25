using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.iOS.Xcode;

public static class MacAppBuilder 
{

	private const string kSettingsPath = "Assets/MacAppBuilder/MacAppBuilderSettings.asset";

	[MenuItem("Tools/MacOS/Build for Mac App Store")]
	private static void MenuBuild()
	{
		string applicationPath = EditorUtility.OpenFilePanel("Select OSX Build","","app");
		if(!string.IsNullOrEmpty(applicationPath)) Build(applicationPath);
	}

	[MenuItem("Tools/MacOS/Create Icon Set")]
	private static void MenuCreateIconSet()
	{
		string iconsPath = EditorUtility.OpenFolderPanel("Select Icon Set Folder","","UnityPlayer.iconset");
		if(!string.IsNullOrEmpty(iconsPath)) CreateIconSet(iconsPath);
	}

	public static void Build(string applicationPath)
	{
		string fileName = Path.GetFileName(applicationPath);
		string applicationDirectory = Path.GetDirectoryName(applicationPath);

		// load build settings
		MacAppBuildSettings settings = AssetDatabase.LoadAssetAtPath<MacAppBuildSettings>(kSettingsPath);
		if(settings==null){
			settings = ScriptableObject.CreateInstance<MacAppBuildSettings>();
			AssetDatabase.CreateAsset(settings,kSettingsPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		// copy player icons
		if(!string.IsNullOrEmpty(settings.playerIconsPath)){
			File.Copy(settings.playerIconsPath,Path.Combine(applicationPath,"Contents/Resources/PlayerIcon.icns"),true);
		}

		// update info.plist
		string infoPlistPath = Path.Combine(applicationPath,"Contents/Info.plist");
		PlistDocument info = new PlistDocument();
		info.ReadFromFile(infoPlistPath);

		PlistElementDict root = info.root;
		root.SetString("CFBundleSignature",PlayerSettings.bundleIdentifier);
		root.SetString("CFBundleIdentifier",PlayerSettings.bundleIdentifier);
		root.SetString("CFBundleGetInfoString",settings.applicationDescription);
		root.SetString("LSApplicationCategoryType",settings.applicationCategory);
		root.SetString("CFBundleShortVersionString",PlayerSettings.bundleVersion);
		root.SetString("CFBundleVersion",PlayerSettings.bundleVersion);

		info.WriteToFile(infoPlistPath);

		// copy entitlements
		string entitlementsName = string.Concat(PlayerSettings.productName,".entitlements");
		if(!string.IsNullOrEmpty(settings.entitlementsPath)){
			File.Copy(settings.entitlementsPath,Path.Combine(applicationDirectory,entitlementsName),true);
		} else {
			throw new UnityException("Entitlements file required");
		}

		// run terminal commands
		ProcessStartInfo startInfo = new ProcessStartInfo("/bin/bash");
		startInfo.WorkingDirectory = applicationDirectory;
		startInfo.UseShellExecute = false;
		startInfo.RedirectStandardInput = true;
		startInfo.RedirectStandardOutput = true;

		Process process = new Process();
		process.StartInfo = startInfo;
		process.Start();

		process.StandardInput.WriteLine(string.Format("chmod -R a+xr {0}",fileName));
		process.StandardInput.WriteLine(string.Format("codesign --force --sign '{2}' --entitlements {0} {1} --deep",entitlementsName,fileName,settings.provisioningProfileApplication));
		process.StandardInput.WriteLine(string.Format("productbuild --component {0} /Applications --sign '{2}' {1}.pkg",fileName,PlayerSettings.productName,settings.provisioningProfileInstaller));

		process.StandardInput.WriteLine("exit");
		process.StandardInput.Flush();

		UnityEngine.Debug.Log(process.StandardOutput.ReadToEnd());

		process.WaitForExit();
	}

	public static void CreateIconSet(string iconsPath)
	{
		string workingDirectory = Directory.GetParent( iconsPath ).ToString();

		// run terminal commands
		ProcessStartInfo startInfo = new ProcessStartInfo("/bin/bash");
		startInfo.WorkingDirectory = workingDirectory;
		startInfo.UseShellExecute = false;
		startInfo.RedirectStandardInput = true;
		startInfo.RedirectStandardOutput = true;

		Process process = new Process();
		process.StartInfo = startInfo;
		process.Start();

		process.StandardInput.WriteLine(string.Format("iconutil -c icns \"{0}\"",iconsPath));
		process.StandardInput.WriteLine("exit");
		process.StandardInput.Flush();

		process.WaitForExit();

		AssetDatabase.Refresh();
	}

}
