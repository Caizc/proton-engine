using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MoreMountains.Tools
{	
	public class InformationAttribute : PropertyAttribute 
	{
		public enum InformationType { Error, Info, None, Warning }

		#if UNITY_EDITOR
	    public string Message;
		public MessageType Type;
		public bool MessageAfterProperty;

		public InformationAttribute(string message, InformationType type, bool messageAfterProperty)
		{
			this.Message = message;
			if (type==InformationType.Error) { this.Type = UnityEditor.MessageType.Error;}
			if (type==InformationType.Info) { this.Type = UnityEditor.MessageType.Info;}
			if (type==InformationType.Warning) { this.Type = UnityEditor.MessageType.Warning;}
			if (type==InformationType.None) { this.Type = UnityEditor.MessageType.None;}
			this.MessageAfterProperty = messageAfterProperty;
		}
		#else
		public InformationAttribute(string message, InformationType type, bool messageAfterProperty)
		{

		}
		#endif
	}
}
