using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.Tools
{	
	/// <summary>
	/// This persistent singleton handles the inputs and sends commands to the player
	/// </summary>
	public class MMControlsTestInputManager : MonoBehaviour
	{
		// on start, we force a high target frame rate for a more fluid experience on mobile devices
		protected virtual void Start()
		{
			Application.targetFrameRate = 300;
		}

		public virtual void LeftJoystickMovement(Vector2 movement) { MMDebug.DebugOnScreen("left joystick",movement); }
		public virtual void RightJoystickMovement(Vector2 movement) { MMDebug.DebugOnScreen("right joystick",movement); }

		public virtual void APressed() { MMDebug.DebugOnScreen("Button A Pressed"); }
		public virtual void BPressed() { MMDebug.DebugOnScreen("Button B Pressed"); }
		public virtual void XPressed() { MMDebug.DebugOnScreen("Button X Pressed"); }
		public virtual void YPressed() { MMDebug.DebugOnScreen("Button Y Pressed"); }
		public virtual void RTPressed()	{ MMDebug.DebugOnScreen("Button RT Pressed"); }

		public virtual void APressedFirstTime() { MMDebug.DebugLogTime("Button A Pressed for the first time"); }
		public virtual void BPressedFirstTime() { MMDebug.DebugLogTime("Button B Pressed for the first time"); }
		public virtual void XPressedFirstTime() { MMDebug.DebugLogTime("Button X Pressed for the first time"); }
		public virtual void YPressedFirstTime() { MMDebug.DebugLogTime("Button Y Pressed for the first time"); }
		public virtual void RTPressedFirstTime() { MMDebug.DebugLogTime("Button RT Pressed for the first time"); }

		public virtual void AReleased()	{ MMDebug.DebugLogTime("Button A Released"); }
		public virtual void BReleased()	{ MMDebug.DebugLogTime("Button B Released"); }
		public virtual void XReleased()	{ MMDebug.DebugLogTime("Button X Released"); }
		public virtual void YReleased()	{ MMDebug.DebugLogTime("Button Y Released"); }
		public virtual void RTReleased()	{ MMDebug.DebugLogTime("Button RT Released"); }

		public virtual void HorizontalAxisPressed(float value) { MMDebug.DebugOnScreen("horizontal movement",value); }
		public virtual void VerticalAxisPressed(float value) { MMDebug.DebugOnScreen("vertical movement",value); }

		public virtual void LeftPressedFirstTime() { MMDebug.DebugLogTime("Button Left Pressed for the first time"); }
		public virtual void UpPressedFirstTime() { MMDebug.DebugLogTime("Button Up Pressed for the first time"); }
		public virtual void DownPressedFirstTime() { MMDebug.DebugLogTime("Button Down Pressed for the first time"); }
		public virtual void RightPressedFirstTime() { MMDebug.DebugLogTime("Button Right Pressed for the first time"); }

		public virtual void LeftReleased()	{ MMDebug.DebugLogTime("Button Left Released"); }
		public virtual void UpReleased()	{ MMDebug.DebugLogTime("Button Up Released"); }
		public virtual void DownReleased()	{ MMDebug.DebugLogTime("Button Down Released"); }
		public virtual void RightReleased()	{ MMDebug.DebugLogTime("Button Right Released"); }

	}
}