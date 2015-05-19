/*
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 *
 * @Filename :		Common.cs
 * @Copyright :		SARL Dreamtronic 2012 - 52265807900028
 * @Author :		Nicolas Lolmède
 * @Version :		2.0
 * @Description :	Fichier rassemblant les fonctions GENERIQUES de GestureWorks
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
	
	public class CommonGW : MonoBehaviour {
		
		// ### PROPERTIES ###############################################################
		
		// Obtention du script "Main" de GestureWorks et du Core
		static private Main						_GWmain;
		static public Main						GWmain {
													get {
														if(_GWmain==null) {
															GameObject mainGO = GameObject.Find("/GestureWorks");
															if(mainGO!=null) {
																_GWmain = mainGO.GetComponent<Main>();
																if(_GWmain!=null) {
																	return _GWmain;
																}
																else {
																	//Debug.Log("[Common] Le component \"main.cs\" n'est pas/plus présent sur le gameObject \"GestureWorks\".");
																	return null;
																}
															}
															else {
																//Debug.Log("[Common] Le gameObject \"GestureWorks\" n'est pas/plus présent sur la scène (time: "+Time.timeSinceLevelLoad+").");
																return null;
															}
														}else {
															return _GWmain;
														}
													}
												}
		static private	GestureworksInstance	_Core;
		static public	GestureworksInstance	Core {
													get{
														if(_Core==null) {
															 _Core = GWmain.Core;
														}
														return _Core;
													}
												}
	}

}