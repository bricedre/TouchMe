using UnityEngine;
using System.Collections;
using System;
using GestureWorksCoreNET;
using TUIO;
using Swifty;

public class GW_touchListener : MonoBehaviour {}

namespace TUIO {
	
	public class Sw_TouchListener : TuioListener {
		
		/**** [STUB] ****/
		
		// This callback method is invoked by the TuioClient when a new TuioObject is added to the session.
		// @param  tobj  the TuioObject reference associated to the addTuioObject event
		public void addTuioObject(TuioObject tobj){}
		
		// This callback method is invoked by the TuioClient when an existing TuioObject is updated during the session.
		// @param  tobj  the TuioObject reference associated to the updateTuioObject event
		public void updateTuioObject(TuioObject tobj){}
		
		// This callback method is invoked by the TuioClient when an existing TuioObject is removed from the session.
		// @param  tobj  the TuioObject reference associated to the removeTuioObject event
		public void removeTuioObject(TuioObject tobj){}
		
		/**** [/STUB] ****/
		
		// This callback method is invoked by the TuioClient when a new TuioCursor is added to the session.
		// @param  tcur  the TuioCursor reference associated to the addTuioCursor event
		public void addTuioCursor(TuioCursor tcur) {
			TouchEvent new_touchEvent = new TouchEvent();
			new_touchEvent.TouchEventID = (int)tcur.getCursorID();
			new_touchEvent.Status = TouchStatus.TOUCHADDED;
			new_touchEvent.X = (float)tcur.getX();
			new_touchEvent.Y = (float)tcur.getY();
			new_touchEvent.Z = 0.0f;
			new_touchEvent.W = 1.0f;
			new_touchEvent.H = 1.0f;
			new_touchEvent.R = 0.0f;
			
			CommonGW.Core.core.AddEvent(new_touchEvent);
		}
		
		// This callback method is invoked by the TuioClient when an existing TuioCursor is updated during the session.
		// @param  tcur  the TuioCursor reference associated to the updateTuioCursor event
		public void updateTuioCursor(TuioCursor tcur) {
			
			TouchEvent updt_touchEvent = new TouchEvent();
			updt_touchEvent.TouchEventID = (int)tcur.getCursorID();
			updt_touchEvent.Status = TouchStatus.TOUCHUPDATE;
			updt_touchEvent.X = (float)tcur.getX();
			updt_touchEvent.Y = (float)tcur.getY();
			updt_touchEvent.Z = 0.0f;
			updt_touchEvent.W = 1.0f;
			updt_touchEvent.H = 1.0f;
			updt_touchEvent.R = 0.0f;
			
			CommonGW.Core.core.AddEvent(updt_touchEvent);
		}
		
		// This callback method is invoked by the TuioClient when an existing TuioCursor is removed from the session.
		// @param  tcur  the TuioCursor reference associated to the removeTuioCursor event
		public void removeTuioCursor(TuioCursor tcur) {
			
			TouchEvent rem_touchEvent = new TouchEvent();
			rem_touchEvent.TouchEventID = (int)tcur.getCursorID();
			rem_touchEvent.Status = TouchStatus.TOUCHREMOVED;
			rem_touchEvent.X = (float)tcur.getX();
			rem_touchEvent.Y = (float)tcur.getY();
			rem_touchEvent.Z = 0.0f;
			rem_touchEvent.W = 1.0f;
			rem_touchEvent.H = 1.0f;
			rem_touchEvent.R = 0.0f;
			
			CommonGW.Core.core.AddEvent(rem_touchEvent);
		}
		
		// This callback method is invoked by the TuioClient to mark the end of a received TUIO message bundle.
		// @param  ftime  the TuioTime associated to the current TUIO message bundle
		public void refresh(TuioTime ftime){}
	}
}