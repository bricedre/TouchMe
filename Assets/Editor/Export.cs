/*
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 *
 * @Filename :		Export.cs
 * @Copyright :		SARL Dreamtronic 2012 - 52265807900028
 * @Author :		Nicolas Lolmède
 * @Version :		2.0
 * @Description :	Script dédié à l'export des fichiers de configuration.
 *
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 *
 * Ce script d'interface doit être présent dans un dossier "Editor" (ou sous-dossier),
 * lui-même placé à la racine de la vue "Project" (dossier "Assets").
 *
 */

using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Callbacks;
using Swifty;

public class Export : EditorWindow {}

// Ajoute le fichier <admin_data.xml> au Build
public class MyBuildPostprocessor {
    
	[PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
		
		/* === OBTENTION DES PATHS === */
		
		// Séparateurs de path
		char[] separators = ("/.").ToCharArray();
		// Découpe le path en elements
		string[] path = pathToBuiltProject.Split( separators[0] );
		// Découpe le nom de l'executable en deux
		string[] exe = path[path.Length-1].Split( separators[1] );
		// Crée et stock les différentes valeurs
		string projectPath = "";
		foreach(string s in path)
			projectPath += (s!=path[path.Length-1] ? s+"/" : "");
		string projectName = exe[0];
		string projectDataPath = projectPath+projectName+"_Data/";

		
		/* === GESTURE WORKS === */
		
		if(target.ToString()=="StandaloneWindows") {
			string gmlFileName = "my_gestures.gml";
			string pathToNewDataFolder = "";
			string pathToAssetsFolder = UnityEngine.Application.dataPath;
			pathToAssetsFolder = pathToAssetsFolder.Replace("/", "\\");
			//destination /Bin folder
			string[] pathPieces = pathToBuiltProject.Split("/".ToCharArray() );
			string exeName = pathPieces[pathPieces.Length-1];
			// extract the name of the exe to use with the name of the data folder
			exeName = exeName.Replace(".exe", "");
			for(int i=1; i<pathPieces.Length; i++){
				// this will grab everything except for the last
				pathToNewDataFolder += pathPieces[i-1]+"\\";
			}
			pathToNewDataFolder += exeName+"_Data\\";
			FileUtil.CopyFileOrDirectory(pathToAssetsFolder+"\\Commons\\Scripts\\GestureWorks\\"+gmlFileName, pathToNewDataFolder+gmlFileName);
		}

		/* === FICHIERS DE CONFIG === */

		// Crée l'arborescence
		Directory.CreateDirectory( projectDataPath+"Settings" );
		
		// Liste des fichiers
		string[] liste = new string[0]{
		};
		
		// Place une copie de chaque fichier dans l'export.
		foreach(string fileName in liste) {
			FileUtil.CopyFileOrDirectory(Application.dataPath+"/Settings/"+fileName+".txt", Path.Combine(projectDataPath, "Settings/"+fileName+".txt"));
		}
	}
}
