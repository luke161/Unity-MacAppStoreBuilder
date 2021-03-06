# Unity-MacAppStoreBuilder
Unity plugin to build and package apps for the Mac App Store. The tools included in this plugin can take an existing OSX app built with Unity and repackage it for Mac App store submission.

## Requirements
* Unity (Built and tested with Unity 5.5)
* XCode (Tested with 8.2.1)
* Application Loader (included with XCode)
* Apple Developer account

## How to use
### 1. Download the plugin
Either grab the latest release `.unitypackage` and import it into your Unity project, or clone a version of the repository and use it as a standalone Unity Project.
### 2. Create an Application ID
If you haven't already you'll need to setup an application in the [Apple Developer Console](https://developer.apple.com/account/mac/identifier/bundle). First make sure you're in the OSX section (selected from the dropdown in the left hand navigation bar) then select *App IDs* and create a new entry. 

If you already have an iOS version of your app it's important that the bundle indentifier you choose is different. 
### 3. Create the required provisioning profiles
You'll need to setup two distribution certificates one for *Mac App Distribution* and the other for *Mac Installer Distribution*. You can do this in XCode (probably the most convenient method) or inside the [Apple Developer Console](https://developer.apple.com/account/mac/certificate/distribution). If you choose the latter option make sure to download the certificates and add them to your keychain.
### 4. Setup your App in iTunes Connect
Once your application is configured in the Apple Developer Console you can create an entry in [iTunes Connect](https://itunesconnect.apple.com/), this is where you'll submit the final package. Create a *New Mac App* from the *My Apps* page, and when prompted select the *Bundle ID* you created in step 2.
### 5. Create an iconset
The iconset generated by Unity for standard OSX builds is missing some of the resolutions required by the Mac App Store. Included in this project is a tool to generate a new icon set (instructions below) or if you have an existing iconset with the required images you can use that (see step 6).
1. Replace the images under `MacAppBuilder/RequiredFiles/UnityPlayer.iconset/` with your own assets, it's important that you use the correct sizes.
2. Run the icon builder from the menu bar `Tools/MacOS/Create Icon Set`
3. Check the generated *.icns* file looks correct `MacAppBuilder/RequiredFiles/UnityPlayer.icns`
### 6. Fill out the build settings
Select the settings asset under `MacAppBuilder/MacAppBuilderSettings.asset` and modify the settings in the inspector.
* **Bundle Indentifier** - set the bundle indentifier to the *Application ID* you created in step 2.
* **Category** - sets the category of your app on the App Store. You can leave it as the default value `public.app-category.games` or pick a specific category from this [list](https://developer.apple.com/library/content/documentation/General/Reference/InfoPlistKeyReference/Articles/LaunchServicesKeys.html#//apple_ref/doc/uid/TP40009250-SW8).
* **Description** - description for your app package.
* **Provisioning profiles** - enter the names of the provisioning profiles created in step 3. You can find the names in the *keychain access* app, under *Certificates*. They should look something like `3rd Party Mac Developer Application: [YOUR COMPANY NAME]`.
* **Player icons** - Set to the *UnityPlayer.icns* file created in step 5, or you can use your own file.
* **Entitlements** - This should already be set to the default entitlements file provided with the project `MacAppBuilder/RequiredFiles/BuildEntitlements.entitlements`. You can edit this file or supply your own if your app requires custom entitlements.
### 7. Run the build tools
Once everythings setup you're ready to package your build! Run `Tools/MacOS/Build for Mac App Store` from the menu bar, you'll be prompted to select your OSX app build. 
Once the process has completed and if everything worked you should see a new *.pkg* file in the same folder as your app.
### 8. Upload the .pkg with Application Loader
Launch *Application Loader*, choose the *.pkg* you just created and start the submission process.

You're done! For future builds you only need to repeat steps 7 & 8.

## Notes
1. When creating an OSX build for the Mac App Store, make sure you have *Mac App Store Validation* enabled in *Player settings*.

## Useful Links
<https://docs.unity3d.com/Manual/HOWTO-PortToAppleMacStore.html>
