using UnityEngine;
using System.Collections;

namespace MoreMountains.Tools
{	
	/// <summary>
	/// Add this class to a GameObject to make it rotate on itself
	/// </summary>
	public class AutoRotate : MonoBehaviour 
	{
		public Space RotationSpace = Space.Self;

		/// The rotation speed. Positive means clockwise, negative means counter clockwise.
		public Vector3 RotationSpeed = new Vector3(100f,0f,0f);

		/// <summary>
		/// Makes the object rotate on its center every frame.
		/// </summary>
		protected virtual void Update () 
		{
			transform.Rotate(RotationSpeed*Time.deltaTime,RotationSpace);
		}
	}
}