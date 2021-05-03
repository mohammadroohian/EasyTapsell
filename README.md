# EasyTapsell

[![Unity 2019.4.7+](https://img.shields.io/badge/unity-2019.4.7%2B-blue.svg)](https://unity3d.com/get-unity/download)
[![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/mohammadroohian/PAction/blob/master/LICENSE)

Easily use [Tapsell](https://tapsell.ir) advertising service!
____________
Features
  * Using [Tapsell](https://tapsell.ir) advertising service without coding.
  * Customizing the default user interface.
  * Drag and drop implementation.

### Why should you use PMBox?

This package allows you to drag and drop several prefabs into a scene to use the Tepsel advertising service.
You can customize the default user interface or use your own.

## Requirements

* Unity 2019.4.7 or later versions.
* [NaughtyAttributes](https://github.com/dbrizov/NaughtyAttributes)
* [PashmakCore](https://github.com/mohammadroohian/PashmakCore)
* [PAction](https://github.com/mohammadroohian/PAction)
* [Tapsell](https://docs.tapsell.ir/plus-sdk/unity/initialize-android/)
* TextMesh Pro 2.0.1
* Post Processing 2.3.0

## Installation
* First install `TextMesh Pro` and `Post Processing` packages in Unity through Package Manager.`MenuItem - Window - Package Manager`
* Add `TextMesh Pro` sample scenes.

### Perform one of the following methods:
#### zip file (The simple way)
1. Download a `source code` zip from [releases](https://github.com/mohammadroohian/EasyTapsell/releases).
2. Extract it.
3. Copy the items in the `Assets` folder into the `Assets` folder of your project. (Click replace files if necessary)

#### unitypackage file
1. Install [NaughtyAttributes](https://github.com/dbrizov/NaughtyAttributes#installation).
2. Install [PashmakCore](https://github.com/mohammadroohian/PashmakCore#installation).
3. Install [PAction](https://github.com/mohammadroohian/PAction#installation).
4. Install [Tapsell](https://docs.tapsell.ir/plus-sdk/unity/initialize-android/)
5. Download `.unitypackage` file from [releases](https://github.com/mohammadroohian/EasyTapsell/releases).
6. Import it into your project.

## Overview
To learn more about how these components work, check out the sample scenes.
The following are a number of practical cases.

![showcase](https://user-images.githubusercontent.com/80090999/116399234-f281db00-a83d-11eb-905d-2edc51e11f23.gif)

### Events
* OnAdCompeleted
* OnAdCanceled
* OnAdAvailable
* OnNoAdAvailable
* OnError
* OnNoNetwork
* OnExpiring

## How to use
1. Drag and drop `Tapsell Manager` to scene from prefabs folder (set [your tapsell key](https://dashboard.tapsell.ir/) into `Tapsell Key` filed).
3. Drag and drop `Tapsell ManagerUI` to scene from prefabs folder.
4. Drag and drop `Tapsell FakeAd Video` to scene from prefabs folder.
5. Customize `Tapsell ManagerUI` itmes if needed (texts and images displayed in messages).

![image](https://user-images.githubusercontent.com/80090999/116891222-b2f32e80-ac43-11eb-8a24-2aab44debf28.png)

6. Use `TapsellEventTrigger` component wherever you need to run specific tasks on mentioned events. for example use `AdCompleted` event to give player gift for watching video.

![trigger](https://user-images.githubusercontent.com/80090999/116892628-3feab780-ac45-11eb-81a2-c0cf405a2f0d.gif)

7. Use `TapsellAdCaller` component to request an ad (set [your zone id](https://dashboard.tapsell.ir/) into `Zone ID` filed).

![USE](https://user-images.githubusercontent.com/80090999/116403125-70e07c00-a842-11eb-9bd6-8e0a141c098c.gif)

### What is FakeAd Video?
This is a panel that is displayed instead of video for debugging and testing, that will not be displayed on the Android build.

![image](https://user-images.githubusercontent.com/80090999/116893399-2c8c1c00-ac46-11eb-9f5e-ccfa712c8de9.png)

