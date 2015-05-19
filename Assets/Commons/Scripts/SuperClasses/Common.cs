/*
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 *
 * @Filename :		Common.cs
 * @Copyright :		SARL Dreamtronic 2012 - 52265807900028
 * @Author :		Nicolas Lolmède
 * @Version :		2.0
 * @Description :	Fichier rassemblant toutes les fonctions GENERIQUES utiles/indispensables.
 *
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 *
 * Ce script peut-être appelé en incluant au sommet des scripts :
 * using Swifty;
 * 
 * Chaque outils ou données peut alors être utilisé(e) en utilisant :
 * Common.nomDeLaFonction();
 * Common.nomDeLaVariable;
 * 
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Security;
using System.Xml;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Swifty {
	
	public class Common : MonoBehaviour {
		
		// ### PROPERTIES ####################################################################
		
		static	public	Hashtable				couleurs = new Hashtable();
		static	public	Hashtable				couleursNoms = new Hashtable();
		static	public	string					kit_name;
		static	public	Dictionary<int,string>	debugOutput = new Dictionary<int,string>();

		
		// ### TOOLS ########################################################################

		// Transforme une <string> en Liste de GameObjects
		static public ArrayList String2ObjectsList(string chaine, string separator) {
			ArrayList listOfGameObjects = new ArrayList();
			char[] separators = separator.ToCharArray();
			string[] paths = chaine.Split( separators[0] );
			int p =0;
			foreach(string path in paths) {
				if(path != "") {
					listOfGameObjects.Add(GameObject.Find(path) as GameObject);
					p++;
				}
			}
			return listOfGameObjects;
		}

		// Edit an axis
		static public Vector3 EditAxisV3(Vector3 vecteur, string axe, float valeur) {
			switch(axe) {
			case "x" :
				return new Vector3( valeur, vecteur.y, vecteur.z );
			case "y" :
				return new Vector3( vecteur.x, valeur, vecteur.z );
			case "z" :
				return new Vector3( vecteur.x, vecteur.y, valeur );
			default :
				return Vector3.zero;
			}
		}

		// Inverse 2 axes dans un Vector3
		static public Vector3 SwitchAxisV3(Vector3 vecteur, string axeA, string axeB) {
			bool fail=false;
			Vector3 res = Vector3.zero;
			switch(axeA) {
				case "x" :
					switch(axeB) {
						case "y" : res = new Vector3( vecteur.y, vecteur.x, vecteur.z );	break;
						case "z" : res = new Vector3( vecteur.z, vecteur.y, vecteur.x );	break;
						default  : fail=true;												break;
					}
					break;
				case "y" :
					switch(axeB) {
						case "x" : res = new Vector3( vecteur.y, vecteur.x, vecteur.z );	break;
						case "z" : res = new Vector3( vecteur.x, vecteur.z, vecteur.y );	break;
						default  : fail=true;												break;
					}
					break;
				case "z" :
					switch(axeB) {
						case "x" : res = new Vector3( vecteur.z, vecteur.y, vecteur.x );	break;
						case "y" : res = new Vector3( vecteur.x, vecteur.z, vecteur.y );	break;
						default  : fail=true;												break;
					}
					break;
				default : fail=true; break;
			}
			if(fail) {
				Debug.Log("[Common][SwitchAxisV3] L'axe indiqué ne correspond à rien !");
				return Vector3.zero;
			}else {
				return res;
			}
		}

		// Genere un ID unique
		static public int GenerateID(){
			byte[] buffer = Guid.NewGuid().ToByteArray();
			return Mathf.Abs( (int)BitConverter.ToInt64(buffer,0) );
		}
		
		// Fermeture technique de l'application
		public static void KillSwifty() {
			
			System.Diagnostics.Process process = new System.Diagnostics.Process();
			System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
			startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
			startInfo.FileName = "cmd.exe";
			startInfo.Arguments = "/C TASKKILL /F /T /IM cmd.exe";
			process.StartInfo = startInfo;
			process.Start();
		}
		
		// Extinction de la table
		public static void ShutDown() {
			System.Diagnostics.Process.Start("shutdown","/s /t 0");
		}
	}

}