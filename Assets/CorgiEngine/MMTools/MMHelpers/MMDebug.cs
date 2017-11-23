using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MoreMountains.Tools
{	
	/// <summary>
	/// Various static methods used throughout the Infinite Runner Engine and the Corgi Engine.
	/// </summary>

	public static class MMDebug 
	{
		public static MMConsole _console; 

		/// <summary>
		/// Draws a debug ray in 2D and does the actual raycast
		/// </summary>
		/// <returns>The raycast hit.</returns>
		/// <param name="rayOriginPoint">Ray origin point.</param>
		/// <param name="rayDirection">Ray direction.</param>
		/// <param name="rayDistance">Ray distance.</param>
		/// <param name="mask">Mask.</param>
		/// <param name="debug">If set to <c>true</c> debug.</param>
		/// <param name="color">Color.</param>
		public static RaycastHit2D RayCast(Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, Color color,bool drawGizmo=false)
		{	
			if (drawGizmo) 
			{
				Debug.DrawRay (rayOriginPoint, rayDirection * rayDistance, color);
			}
			return Physics2D.Raycast(rayOriginPoint,rayDirection,rayDistance,mask);		
		}


		public static RaycastHit2D MonoRayCastNonAlloc(RaycastHit2D[] array, Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, Color color,bool drawGizmo=false)
		{	
			if (drawGizmo) 
			{
				Debug.DrawRay (rayOriginPoint, rayDirection * rayDistance, color);
			}
			if (Physics2D.RaycastNonAlloc(rayOriginPoint, rayDirection, array, rayDistance, mask) > 0)
			{
				return array[0];
			}
			return new RaycastHit2D();        	
		}

		/// <summary>
		/// Draws a debug ray in 3D and does the actual raycast
		/// </summary>
		/// <returns>The raycast hit.</returns>
		/// <param name="rayOriginPoint">Ray origin point.</param>
		/// <param name="rayDirection">Ray direction.</param>
		/// <param name="rayDistance">Ray distance.</param>
		/// <param name="mask">Mask.</param>
		/// <param name="debug">If set to <c>true</c> debug.</param>
		/// <param name="color">Color.</param>
		/// <param name="drawGizmo">If set to <c>true</c> draw gizmo.</param>
		public static RaycastHit Raycast3D(Vector3 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, Color color,bool drawGizmo=false)
		{
			if (drawGizmo) 
			{
				Debug.DrawRay (rayOriginPoint, rayDirection * rayDistance, color);
			}
			RaycastHit hit;
			Physics.Raycast(rayOriginPoint,rayDirection,out hit,rayDistance,mask);	
			return hit;
		}

		/// <summary>
		/// Outputs the message object to the console, prefixed with the current timestamp
		/// </summary>
		/// <param name="message">Message.</param>
		public static void DebugLogTime(object message, string color="")
		{
			string colorPrefix = "";
			string colorSuffix = "";
			if (color != "")
			{
				colorPrefix = "<color="+color+">";
				colorSuffix = "</color>";
			}
			Debug.Log (colorPrefix + Time.time + " " + message + colorSuffix);

		}

		/// <summary>
		/// Instantiates a MMConsole if there isn't one already, and adds the message in parameter to it.
		/// </summary>
		/// <param name="message">Message.</param>
		public static void DebugOnScreen(string message)
		{
			InstantiateOnScreenConsole();
			_console.AddMessage(message);
		}

		/// <summary>
		/// Instantiates a MMConsole if there isn't one already, and displays the label in bold and its value next to it.
		/// </summary>
		/// <param name="label">Label.</param>
		/// <param name="value">Value.</param>
		/// <param name="fontSize">The optional font size.</param>
		public static void DebugOnScreen(string label, object value, int fontSize=10)
		{
			InstantiateOnScreenConsole(fontSize);
			_console.AddMessage("<b>"+label+"</b> : "+value);
		}

		/// <summary>
		/// Instantiates the on screen console if there isn't one already
		/// </summary>
		public static void InstantiateOnScreenConsole(int fontSize=10)
		{
			if (_console == null)
			{
				// we instantiate the console
				GameObject newGameObject = new GameObject();
				newGameObject.name="MMConsole";
				_console = newGameObject.AddComponent<MMConsole>();
				_console.SetFontSize(fontSize);
			}
		}

		/// <summary>
		/// Draws a gizmo arrow going from the origin position and along the direction Vector3
		/// </summary>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="color">Color.</param>
	    public static void GizmosDrawArrow(Vector3 origin, Vector3 direction, Color color)
	    {
			float arrowHeadLength = 3.00f;
			float arrowHeadAngle = 25.0f;

	        Gizmos.color = color;
	        Gizmos.DrawRay(origin, direction);
	       
			DrawArrowEnd(true,origin,direction,color,arrowHeadLength,arrowHeadAngle);
	    }

	    /// <summary>
		/// Draws a debug arrow going from the origin position and along the direction Vector3
	    /// </summary>
	    /// <param name="origin">Origin.</param>
	    /// <param name="direction">Direction.</param>
	    /// <param name="color">Color.</param>
	    public static void DebugDrawArrow(Vector3 origin, Vector3 direction, Color color)
	    {
			float arrowHeadLength = 0.20f;
			float arrowHeadAngle = 35.0f;

	        Debug.DrawRay(origin, direction, color);
	       
			DrawArrowEnd(false,origin,direction,color,arrowHeadLength,arrowHeadAngle);
	    }


		private static void DrawArrowEnd (bool drawGizmos, Vector3 arrowEndPosition, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 40.0f)
	    {
			if (direction == Vector3.zero)
			{
				return;
			}
	        Vector3 right = Quaternion.LookRotation (direction) * Quaternion.Euler (arrowHeadAngle, 0, 0) * Vector3.back;
	        Vector3 left = Quaternion.LookRotation (direction) * Quaternion.Euler (-arrowHeadAngle, 0, 0) * Vector3.back;
	        Vector3 up = Quaternion.LookRotation (direction) * Quaternion.Euler (0, arrowHeadAngle, 0) * Vector3.back;
	        Vector3 down = Quaternion.LookRotation (direction) * Quaternion.Euler (0, -arrowHeadAngle, 0) * Vector3.back;
	        if (drawGizmos) 
	        {
	            Gizmos.color = color;
	            Gizmos.DrawRay (arrowEndPosition + direction, right * arrowHeadLength);
	            Gizmos.DrawRay (arrowEndPosition + direction, left * arrowHeadLength);
	            Gizmos.DrawRay (arrowEndPosition + direction, up * arrowHeadLength);
	            Gizmos.DrawRay (arrowEndPosition + direction, down * arrowHeadLength);
	        }
	        else
	        {
	            Debug.DrawRay (arrowEndPosition + direction, right * arrowHeadLength, color);
	            Debug.DrawRay (arrowEndPosition + direction, left * arrowHeadLength, color);
	            Debug.DrawRay (arrowEndPosition + direction, up * arrowHeadLength, color);
	            Debug.DrawRay (arrowEndPosition + direction, down * arrowHeadLength, color);
	        }
	    }

		/// <summary>
		/// Draws handles to materialize the bounds of an object on screen.
		/// </summary>
		/// <param name="bounds">Bounds.</param>
		/// <param name="color">Color.</param>
		public static void DrawHandlesBounds(Bounds bounds, Color color)
		{
			#if UNITY_EDITOR
			Vector3 boundsCenter = bounds.center;
		    Vector3 boundsExtents = bounds.extents;
		  
			Vector3 v3FrontTopLeft     = new Vector3(boundsCenter.x - boundsExtents.x, boundsCenter.y + boundsExtents.y, boundsCenter.z - boundsExtents.z);  // Front top left corner
			Vector3 v3FrontTopRight    = new Vector3(boundsCenter.x + boundsExtents.x, boundsCenter.y + boundsExtents.y, boundsCenter.z - boundsExtents.z);  // Front top right corner
			Vector3 v3FrontBottomLeft  = new Vector3(boundsCenter.x - boundsExtents.x, boundsCenter.y - boundsExtents.y, boundsCenter.z - boundsExtents.z);  // Front bottom left corner
			Vector3 v3FrontBottomRight = new Vector3(boundsCenter.x + boundsExtents.x, boundsCenter.y - boundsExtents.y, boundsCenter.z - boundsExtents.z);  // Front bottom right corner
			Vector3 v3BackTopLeft      = new Vector3(boundsCenter.x - boundsExtents.x, boundsCenter.y + boundsExtents.y, boundsCenter.z + boundsExtents.z);  // Back top left corner
			Vector3 v3BackTopRight     = new Vector3(boundsCenter.x + boundsExtents.x, boundsCenter.y + boundsExtents.y, boundsCenter.z + boundsExtents.z);  // Back top right corner
			Vector3 v3BackBottomLeft   = new Vector3(boundsCenter.x - boundsExtents.x, boundsCenter.y - boundsExtents.y, boundsCenter.z + boundsExtents.z);  // Back bottom left corner
			Vector3 v3BackBottomRight  = new Vector3(boundsCenter.x + boundsExtents.x, boundsCenter.y - boundsExtents.y, boundsCenter.z + boundsExtents.z);  // Back bottom right corner


			Handles.color = color;

			Handles.DrawLine (v3FrontTopLeft, v3FrontTopRight);
			Handles.DrawLine (v3FrontTopRight, v3FrontBottomRight);
			Handles.DrawLine (v3FrontBottomRight, v3FrontBottomLeft);
			Handles.DrawLine (v3FrontBottomLeft, v3FrontTopLeft);
		         
			Handles.DrawLine (v3BackTopLeft, v3BackTopRight);
			Handles.DrawLine (v3BackTopRight, v3BackBottomRight);
			Handles.DrawLine (v3BackBottomRight, v3BackBottomLeft);
			Handles.DrawLine (v3BackBottomLeft, v3BackTopLeft);
		         
			Handles.DrawLine (v3FrontTopLeft, v3BackTopLeft);
			Handles.DrawLine (v3FrontTopRight, v3BackTopRight);
			Handles.DrawLine (v3FrontBottomRight, v3BackBottomRight);
			Handles.DrawLine (v3FrontBottomLeft, v3BackBottomLeft);  
			#endif
		}

		/// <summary>
		/// Draws a gizmo sphere of the specified size and color at a position
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="size">Size.</param>
		/// <param name="color">Color.</param>
		public static void DrawGizmoPoint(Vector3 position, float size, Color color)
		{
	    	Gizmos.color = color;
			Gizmos.DrawWireSphere(position,size);
		}

		public static void DrawGizmoRectangle(Vector2 center, Vector2 size, Color color)
		{
			Gizmos.color = color;

			Vector3 v3TopLeft = new Vector3(center.x - size.x/2, center.y + size.y/2, 0);
			Vector3 v3TopRight = new Vector3(center.x + size.x/2, center.y + size.y/2, 0);;
			Vector3 v3BottomRight = new Vector3(center.x + size.x/2, center.y - size.y/2, 0);;
			Vector3 v3BottomLeft = new Vector3(center.x - size.x/2, center.y - size.y/2, 0);;

			Gizmos.DrawLine(v3TopLeft,v3TopRight);
			Gizmos.DrawLine(v3TopRight,v3BottomRight);
			Gizmos.DrawLine(v3BottomRight,v3BottomLeft);
			Gizmos.DrawLine(v3BottomLeft,v3TopLeft);
		}


	}
}
