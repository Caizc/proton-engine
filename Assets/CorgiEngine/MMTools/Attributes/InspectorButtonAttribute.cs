﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;

namespace MoreMountains.Tools
{	
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public class InspectorButtonAttribute : PropertyAttribute
	{
		public readonly string MethodName;

		public InspectorButtonAttribute(string MethodName)
		{
			this.MethodName = MethodName;
		}
	}

	#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(InspectorButtonAttribute))]
	public class InspectorButtonPropertyDrawer : PropertyDrawer
	{
		private MethodInfo _eventMethodInfo = null;

		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			InspectorButtonAttribute inspectorButtonAttribute = (InspectorButtonAttribute)attribute;

			float buttonLength = 50 + inspectorButtonAttribute.MethodName.Length * 6;
			Rect buttonRect = new Rect(position.x + (position.width - buttonLength) * 0.5f, position.y, buttonLength, position.height);

			if (GUI.Button(buttonRect, inspectorButtonAttribute.MethodName))
			{
				System.Type eventOwnerType = prop.serializedObject.targetObject.GetType();
				string eventName = inspectorButtonAttribute.MethodName;

				if (_eventMethodInfo == null)
				{
					_eventMethodInfo = eventOwnerType.GetMethod(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}

				if (_eventMethodInfo != null)
				{
					_eventMethodInfo.Invoke(prop.serializedObject.targetObject, null);
				}
				else
				{
					Debug.LogWarning(string.Format("InspectorButton: Unable to find method {0} in {1}", eventName, eventOwnerType));
				}
			}
		}
	}
	#endif
}