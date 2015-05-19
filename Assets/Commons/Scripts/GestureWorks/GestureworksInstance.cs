using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GestureWorksCoreNET;
//using Swifty;

/// <summary>
/// This is just a test of the Gestureworks singleton. You will need to fill out the rest of the methods as outlined in the wiki. In order to run this demo, you will need the core dlls.
/// </summary>

public sealed class GestureworksInstance {

		// instance
		public static readonly GestureworksInstance	instance = new GestureworksInstance();

		// core
		public	GestureWorks			core;
		
		// settings
		private	bool					verboseMode;
		private	string					windowName_inPlayer;
		private	bool					PQLab;
		
		// integration
		private	string					windowName;
		private	string					dllFileName;
		private	string					gmlFileName;
		private	string					dllFilePath;
		private	string					gmlFilePath;
		
		// status
		private	Main					GWmain;
		public	bool					DllLoaded;
		public	bool					GmlLoaded;
		public	bool					WindowLoaded;
		
		// Datas						
		public PointEventArray			pEvents = new PointEventArray();
		public GestureEventArray		gEvents = new GestureEventArray();
		public List<GestureWorksCoreNET.Unity.TouchObject>
										GestureObjects = new List<GestureWorksCoreNET.Unity.TouchObject>();
		public Dictionary<int, GestureWorksCoreNET.Unity.TouchCircle>
										TouchPoints = new Dictionary<int, GestureWorksCoreNET.Unity.TouchCircle>();

		
	public GestureworksInstance() {
		/*
		 * 32 ou 64 :
		 * Commons/Scripts/GestureWorks/Sources/GwNative.cs @ 34
		 * Editor/SwiftyMenu/_SwiftyMenu.cs @ 59
		 * Here @ 60
		 */
		
		GWmain = GameObject.Find("/GestureWorks").GetComponent<Main>();
		verboseMode = GWmain.verboseMode;
		windowName_inPlayer = GWmain.windowName_inPlayer;
		PQLab = GWmain.PQLab;
		
		dllFileName = "GestureworksCore32.dll";//"GestureworksCore64.dll";
		gmlFileName = "my_gestures.gml";
		
		if(Application.isEditor==true) {
			// Running in editor
			windowName = "Unity - " + (Application.loadedLevelName) + ".unity - " + GestureworksInstance.getFolderName() + " - PC, Mac & Linux Standalone*";//" <DX11>";
			//dllFilePath = Application.dataPath.Replace("/", "\\")+"\\Plugins\\GestureWorks\\Core\\"+dllFileName;
			dllFilePath = "C:\\Swifty\\dll\\gestureworks\\"+dllFileName;
			gmlFilePath = Application.dataPath.Replace("/", "\\")+"\\Commons\\Scripts\\GestureWorks\\"+gmlFileName;
		}else {
			// Running in standalone player
			windowName = windowName_inPlayer;
			//dllFilePath = Application.dataPath.Replace("/", "\\")+"\\Managed\\"+dllFileName;
			dllFilePath = "C:\\Swifty\\dll\\gestureworks\\"+dllFileName;
			gmlFilePath = Application.dataPath.Replace("/", "\\")+"\\"+gmlFileName;
		}
		if(verboseMode) Debug.Log("Window Name: "+windowName);
		
		core = new GestureWorks();
		
		DllLoaded = core.LoadGestureWorksDll(dllFilePath);
			if(verboseMode) Debug.Log("DllLoaded (from "+dllFilePath+" ): "+DllLoaded);
		
		core.InitializeGestureWorks( (PQLab? Screen.width/2: Screen.width), Screen.height);
		
		GmlLoaded = core.LoadGML(gmlFilePath);
			if(verboseMode) Debug.Log("Is GML Loaded (from "+gmlFilePath+"): " + GmlLoaded.ToString());
		
		WindowLoaded = core.RegisterWindowForTouchByName(windowName);
		if (verboseMode) {
			Debug.Log("Is Window Loaded: " + WindowLoaded.ToString());
		}
		if (!WindowLoaded) {
			Debug.Log("Window name incorrect: " + windowName + "\n Try to add <DX11>...");
			WindowLoaded = core.RegisterWindowForTouchByName(windowName + " <DX11>");
		}
		if (verboseMode) {
			Debug.Log("Is Window Loaded with <DX11>: " + WindowLoaded.ToString());
		}
	}
 
	public static GestureworksInstance Instance {
		get {
			return instance;
		}
	}

	public void initOrCleanDatas() {
		// UNregister objects
		GestureWorksCoreNET.Unity.TouchObject[] touchObjects = GameObject.FindObjectsOfType(typeof(GestureWorksCoreNET.Unity.TouchObject)) as GestureWorksCoreNET.Unity.TouchObject[];
		foreach(GestureWorksCoreNET.Unity.TouchObject obj in touchObjects) {
			DeregisterTouchObject(obj);
		}
		// Clean pEvents
		instance.pEvents.Clear();
		// Clean gEvents
		instance.gEvents.Clear();
		// Clean Objects
		instance.GestureObjects.Clear();
		// Clean TouchPoints
		instance.TouchPoints.Clear();
	}

	private void DeregisterTouchObject(GestureWorksCoreNET.Unity.TouchObject obj) {
		string objName = obj.GetObjectName();
		core.DeregisterTouchObject(objName);
		foreach(string gesture in obj.SupportedGestures) {
			core.DisableGesture(objName, gesture);
			core.RemoveGesture(objName, gesture);
		}
	}

	static public string getFolderName() {
		string separator = "/";
		string path = Application.dataPath;
		ArrayList pathWords = new ArrayList();
		char[] separators = separator.ToCharArray();
		string[] words = path.Split(separators[0]);
		int p = 0;
		foreach (string word in words) {
			if (word != "") {
				pathWords.Add(word as string);
				p++;
			}
		}
		return (string)pathWords[pathWords.Count - 2];
	}
}