/*
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 *
 * @Filename :		TouchToParent.cs
 * @Copyright :		SARL Dreamtronic 2012 - 52265807900028
 * @Author :		Nicolas Lolmède
 * @Version :		1.0
 * @Description :	Renvoi les évènements GestureWorks au GameObject parent.
 *
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 *
 * Ce script peut être présent plusieurs fois dans la scène, ùmais une seule fois par gameObject.
 * 
 * Paramètres disponibles :
 * - X
 *
 */

using UnityEngine;
using System.Collections;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;
using Swifty;

public class TouchToParent : TouchObject
{

    // ### PROPERTIES ####################################################################

    // Accessibles dans l'inspecteur
    /* Propriétés publiques, visibles dans l'inspecteur */

    // Accessibles dans d'autres scripts
    /* Propriétés publiques, mais avec {get/set}, et donc non visibles en inspecteur */

    // Privées
    /* Propriétés strictement privées */
    private GameObject papou;


    // ### INITIALISATION ################################################################
    void Awake()
    {
        papou = this.transform.parent.gameObject;
        if (!papou)
        {
            Debug.Log("[TouchToParent] Pas de parent !");
            Destroy(this);
        }
    }


    // ### 1ST FRAME #####################################################################
    //void Start() {}


    // ### UPDATE ########################################################################
    //void Update() {}


    // ### MULTITOUCH ####################################################################

    public void set_pEvent(int pointID)
    {
        papou.SendMessage("set_pEvent", pointID, SendMessageOptions.DontRequireReceiver);
    }

    public void NDrag(GestureEvent gEvent)
    {
        papou.SendMessage("NDrag", gEvent, SendMessageOptions.DontRequireReceiver);
    }

    public void Drag(GestureEvent gEvent)
    {
        papou.SendMessage("Drag", gEvent, SendMessageOptions.DontRequireReceiver);
    }

    // Rotate à un seul doigt
    public void Rotate(GestureEvent gEvent)
    {
        papou.SendMessage("Rotate", gEvent, SendMessageOptions.DontRequireReceiver);
    }

    // Rotate à plusieurs doigts
    public void NRotate(GestureEvent gEvent)
    {
        papou.SendMessage("NRotate", gEvent, SendMessageOptions.DontRequireReceiver);
    }

    // Scale à un seul doigt
    public void Scale(GestureEvent gEvent)
    {
        papou.SendMessage("Scale", gEvent, SendMessageOptions.DontRequireReceiver);
    }

    // Scale à plusieurs doigts
    public void NScale(GestureEvent gEvent)
    {
        papou.SendMessage("NScale", gEvent, SendMessageOptions.DontRequireReceiver);
    }
    // ### TOOLS #########################################################################

}