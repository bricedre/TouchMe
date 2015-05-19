<?xml version="1.0" encoding="UTF-8"?>
<GestureMarkupLanguage xmlns:gml="http://gestureworks.com/gml/version/1.0">

	<!-- Gestures basiques -->
	<Gesture_set gesture_set_name="basic-gestures">
		
		<!-- Gesture "DRAG" sans inertie -->
		<Gesture id="Drag" type="drag">
			<comment>The 'drag' gesture can be activated by one touch point. When a touch down is recognized on a touch object the position
			of the touch point is tracked. This change in the position of the touch point is mapped directly to the position of the touch object.</comment>
			<match>
				<action>
					<initial>
						<cluster point_number="0" point_number_min="1" point_number_max="1"/>
					</initial>
				</action>
			</match>	
			<analysis>
				<algorithm class="kinemetric" type="continuous">
					<library module="drag"/>
					<returns>
						<property id="drag_dx" result="dx"/>
						<property id="drag_dy" result="dy"/>
					</returns>
				</algorithm>
			</analysis>	
			<mapping>
				<update dispatch_type="continuous">
					<gesture_event type="drag">
						<property ref="drag_dx" target="x"/>
						<property ref="drag_dy" target="y"/>
					</gesture_event>
				</update>
			</mapping>
		</Gesture>
		
		<!-- Gesture "DRAG" avec inertie -->
		<Gesture id="NDrag" type="drag">
			<comment>The 'n-drag' gesture can be activated by any number of touch points. When a touch down is recognized on a touch object the position
			of the touch point is tracked. This change in the position of the touch point is mapped directly to the position of the touch object.</comment>
			<match>
				<action>
					<initial>
						<cluster point_number="0" point_number_min="1" point_number_max="10"/>
					</initial>
				</action>
			</match>	
			<analysis>
				<algorithm class="kinemetric" type="continuous">
					<library module="drag"/>
					<returns>
						<property id="drag_dx" result="dx"/>
						<property id="drag_dy" result="dy"/>
					</returns>
				</algorithm>
			</analysis>	
			<processing>
				<inertial_filter>
					<property ref="drag_dx" active="true" friction="0.9"/>
					<property ref="drag_dy" active="true" friction="0.9"/>
				</inertial_filter>
				<delta_filter>
					<property ref="drag_dx" active="true" delta_min="0.05" delta_max="500"/>
					<property ref="drag_dy" active="true" delta_min="0.05" delta_max="500"/>
				</delta_filter>
			</processing>
			<mapping>
				<update dispatch_type="continuous">
					<gesture_event type="drag">
						<property ref="drag_dx" target="x"/>
						<property ref="drag_dy" target="y"/>
					</gesture_event>
				</update>
			</mapping>
		</Gesture>

		<!-- Gesture "HorizontalStroke" -->
		<Gesture id="HorizontalStroke" type="stroke">
		  <match>
		          <action>
		                  <initial>
		                          <point path_pts="(x=0.0, y=0.0),(x=1.0, y=0.0)"/>
		                          <cluster point_number="1"/>
		                          <!--<event touchEvent="TouchEnd"/>-->
		                  </initial>
		          </action>
		  </match>
		  <analysis>
		          <algorithm class="vectormetric" type="continuous">
		                  <library module="stroke"/>
		                  <returns>
		                          <property id="stroke_x" result="x"/>
		                          <property id="stroke_y" result="y"/>
		                          <property id="stroke_prob" result="prob"/>
		                  </returns>
		          </algorithm>
		  </analysis>
		  <mapping>
		          <!--<update dispatch_type="discrete" dispatch_mode="cluster_remove">-->
                  <update dispatch_type="continuous">
		                  <gesture_event type="stroke_letter">
		                          <property ref="stroke_x"/>
		                          <property ref="stroke_y"/>
		                          <property ref="stroke_prob"/>
		                  </gesture_event>
		          </update>
		  </mapping>
		</Gesture>
		
		<!-- Gesture "ThreeFingerTilt" -->
		<Gesture id="ThreeFingerTilt" type="tilt">
			<comment></comment>
			<match>
				<action>
					<initial>
						<cluster point_number="3" point_number_min="3" point_number_max="3" separation_min="0.01"/>
					</initial>
				</action>
			</match>
				<analysis>
					<algorithm class="kinemetric" type="continuous">
						<library module="tilt"/>
						<returns>
							<property id="tilt_dx" result="dsx"/>
							<property id="tilt_dy" result="dsy"/>
						</returns>
					</algorithm>
				</analysis>
			<processing>
				<noise_filter>
					<property ref="tilt_dx" noise_filter="false" percent="0"/>
					<property ref="tilt_dy" noise_filter="false" percent="0"/>
				</noise_filter>
				<inertial_filter>
					<property ref="tilt_dx" release_inertia="false" friction="0"/>
					<property ref="tilt_dy" release_inertia="false" friction="0"/>
				</inertial_filter>
				<delta_filter>
					<property ref="tilt_dx" delta_threshold="false" delta_min="0.0001" delta_max="1"/>
					<property ref="tilt_dy" delta_threshold="false" delta_min="0.0001" delta_max="1"/>
				</delta_filter>
			</processing>
			<mapping>
				<update dispatch_type="continuous">
					<gesture_event>
						<property ref="tilt_dx" target=""/>
						<property ref="tilt_dy" target=""/>
					</gesture_event>
				</update>
			</mapping>
		</Gesture>
		
		<!-- Gesture "NROTATE" -->
		<Gesture id="NRotate" type="rotate">
			<comment></comment>
			<match>
				<action>
					<initial>
						<cluster point_number="0" point_number_min="2" point_number_max="10"/>
					</initial>
				</action>
			</match>
			<analysis>
				<algorithm class="kinemetric" type="continuous">
					<library module="rotate"/>
					<returns>
						<property id="rotate_dtheta" result="dtheta"/>
					</returns>
				</algorithm>
			</analysis>	
			<processing>
				<inertial_filter>
					<property ref="rotate_dtheta" active="true" friction="0.9"/>
				</inertial_filter>
				<delta_filter>
					<property ref="rotate_dtheta" active="true" delta_min="0.01" delta_max="20"/>
				</delta_filter>
			</processing>
			<mapping>
				<update dispatch_type="continuous">
					<gesture_event type="rotate">
						<property ref="rotate_dtheta" target="rotate"/>
					</gesture_event>
				</update>
			</mapping>
		</Gesture>
		
		<!-- Gesture "NSCALE" with 2 FINGERS -->
		<Gesture id="NScale" type="scale">
			<comment></comment>
			<match>
				<action>
					<initial>
						<cluster point_number="2"/>
					</initial>
				</action>
			</match>
			<analysis>
				<algorithm class="kinemetric" type="continuous">
					<library module="scale"/>
					<returns>
						<property id="scale_dsx" result="ds"/>
						<property id="scale_dsy" result="ds"/>
					</returns>
				</algorithm>
			</analysis>	
			<processing>
				<inertial_filter>
					<property ref="scale_dsx" active="true" friction="0.9"/>
					<property ref="scale_dsy" active="true" friction="0.9"/>
				</inertial_filter>
				<delta_filter>
					<property ref="scale_dsx" active="false" delta_min="0.0001" delta_max="1"/>
					<property ref="scale_dsy" active="false" delta_min="0.0001" delta_max="1"/>
				</delta_filter>
				<multiply_filter>
					<property ref="scale_dsx" active="true" func="linear" factor="0.0033"/>
					<property ref="scale_dsy" active="true" func="linear" factor="0.0033"/>
				</multiply_filter>
			</processing>
			<mapping>
				<update dispatch_type="continuous">
					<gesture_event type="scale">
						<property ref="scale_dsx" target="scaleX"/>
						<property ref="scale_dsy" target="scaleY"/>
					</gesture_event>
				</update>
			</mapping>
		</Gesture>
		
		<!-- Gesture "NSCALE2" with inerty & N fingers -->
		<Gesture id="NScale2" type="scale">
			<comment></comment>
			<match>
				<action>
					<initial>
						<cluster point_number="0" point_number_min="2" point_number_max="10"/>
					</initial>
				</action>
			</match>
			<analysis>
				<algorithm class="kinemetric" type="continuous">
					<library module="scale"/>
					<returns>
						<property id="scale_dsx" result="ds"/>
						<property id="scale_dsy" result="ds"/>
					</returns>
				</algorithm>
			</analysis>	
			<processing>
				<inertial_filter>
					<property ref="scale_dsx" active="true" friction="0.9"/>
					<property ref="scale_dsy" active="true" friction="0.9"/>
				</inertial_filter>
				<delta_filter>
					<property ref="scale_dsx" active="false" delta_min="0.0001" delta_max="1"/>
					<property ref="scale_dsy" active="false" delta_min="0.0001" delta_max="1"/>
				</delta_filter>
				<multiply_filter>
					<property ref="scale_dsx" active="true" func="linear" factor="0.0033"/>
					<property ref="scale_dsy" active="true" func="linear" factor="0.0033"/>
				</multiply_filter>
			</processing>
			<mapping>
				<update dispatch_type="continuous">
					<gesture_event type="scale">
						<property ref="scale_dsx" target="scaleX"/>
						<property ref="scale_dsy" target="scaleY"/>
					</gesture_event>
				</update>
			</mapping>
		</Gesture>
		
	</Gesture_set>
	
	
	
	
	<!-- Gestures temporelles -->
	<Gesture_set gesture_set_name="temporal-gestures">
	
		<!-- Gesture "CLICK" -->
		<Gesture id="Tap" type="tap">
			<comment></comment>
			<match>
				<action>
					<initial>
						<point event_duration_max="100" translation_max="10"/>
						<cluster point_number="1"/>
						<event touch_event="touchEnd"/>
					</initial>
				</action>
			</match>
			<analysis>
				<algorithm class="temporalmetric" type="discrete">
					<library module="tap"/>
					<returns>
						<property id="tap_x" result="x"/>
						<property id="tap_y" result="y"/>
						<property id="tap_n" result="n"/>
					</returns>
				</algorithm>
			</analysis>
			<mapping>
				<update dispatch_type="discrete" dispatch_mode="batch" dispatch_interval="1000">
					<gesture_event>
						<property ref="tap_x"/>
						<property ref="tap_y"/>
						<property ref="tap_n"/>
					</gesture_event>
				</update>
			</mapping>
		</Gesture>

		<!-- Gesture "PRESS" -->
		<Gesture id="Press" type="hold">
			<comment></comment>
			<match>
				<action>
					<initial>
						<point event_duration_min="100" translation_max="10"/>
						<cluster point_number="1"/>
					</initial>
				</action>
			</match>      
			<analysis>
				<algorithm>
					<library module="hold"/>
					<returns>
						<property id="hold_x" result="x"/>
						<property id="hold_y" result="y"/>
						<property id="hold_n" result="n"/>
					</returns>
				</algorithm>
			</analysis>    
			<mapping>
				<update dispatch_type="discrete" dispatch_reset="cluster_remove">
					<gesture_event  type="hold">
						<property ref="hold_x"/>
						<property ref="hold_y"/>
						<property ref="hold_n"/>
					</gesture_event>
				</update>
			</mapping>
		</Gesture>
		
		<!-- Gesture "FLICK" -->
		<Gesture id="Flick" type="flick">
			<comment>The 'n-flick' gesture can be activated by one touch points. When a touch down is recognized on a touch object, the velocity and
			acceleration of the touch point are tracked. If acceleration of the cluster is above the acceleration threshold a flick event is dispatched.</comment>
			<match>
				<action>
					<initial>
						<cluster point_number="1" acceleration_min="0.5"/>
						<event touch_event="touchEnd"/>
					</initial>
				</action>
			</match>
			<analysis>
				<algorithm class="kinemetric" type="continuous">
					<library module="flick"/>
					<variables>
						<property id="flick_dx" var="etm_ddx" return="etm_dx" var_min="2"/>
						<property id="flick_dy" var="etm_ddy" return="etm_dy" var_min="2"/>
					</variables>
					<returns>
						<property id="flick_dx" result="etm_dx"/>
						<property id="flick_dy" result="etm_dy"/>
					</returns>
				</algorithm>
			</analysis>    
			<mapping>
				<update dispatch_type="discrete" dispatch_mode="cluster_remove" dispatch_reset="cluster_remove">
					<gesture_event  type="flick">
						<property ref="flick_dx" target=""/>
						<property ref="flick_dy" target=""/>
					</gesture_event>
				</update>
			</mapping>
		</Gesture>
		
		<!-- Gesture "SWIPE" -->
		<Gesture id="Swipe" type="swipe">
			<comment>The 'Swipe' gesture can be activated by one touch point. When a touch down is recognized on a touch object, the velocity and
			acceleration of the touch point are tracked. If acceleration of the cluster is below the acceleration threshold a swipe event is dispatched.</comment>
			<match>
				<action>
					<initial>
						<cluster point_number="1" acceleration_max="150.5"/>
						<event touch_event="touchEnd"/>
					</initial>
				</action>
			</match>
			<analysis>
				<algorithm class="kinemetric" type="continuous">
					<library module="swipe" />
					<variables>
						<property id="swipe_dx" var="etm_ddx" return="etm_dx" var_max="1"/>
						<property id="swipe_dy" var="etm_ddy" return="etm_dy" var_max="1"/>
					</variables>
					<returns>
						<property id="swipe_dx" result="etm_dx"/>
						<property id="swipe_dy" result="etm_dy"/>
					</returns>
				</algorithm>
			</analysis>  
			<mapping>
				<update dispatch_type="discrete" dispatch_mode="cluster_remove" dispatch_reset="cluster_remove">
					<gesture_event  type="swipe">
						<property ref="swipe_dx" target=""/>
						<property ref="swipe_dy" target=""/>
					</gesture_event>
				</update>
			</mapping>
		</Gesture>
		
	</Gesture_set>
	
</GestureMarkupLanguage>