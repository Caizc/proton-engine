using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MoreMountains.Tools
{	
	/// <summary>
	/// Contains all extension methods of the Corgi Engine and Infinite Runner Engine.
	/// </summary>
	public static class ExtensionMethods 
	{
		/// <summary>
		/// Determines if an animator contains a certain parameter, based on a type and a name
		/// </summary>
		/// <returns><c>true</c> if has parameter of type the specified self name type; otherwise, <c>false</c>.</returns>
		/// <param name="self">Self.</param>
		/// <param name="name">Name.</param>
		/// <param name="type">Type.</param>
		public static bool HasParameterOfType (this Animator self, string name, AnimatorControllerParameterType type) 
		{
			if (name == null || name == "") { return false; }
			AnimatorControllerParameter[] parameters = self.parameters;
			foreach (AnimatorControllerParameter currParam in parameters) 
			{
				if (currParam.type == type && currParam.name == name) 
				{
					return true;
				}
			}
			return false;
		}	


        public static bool Contains(this LayerMask mask, int layer) 
        {
             return ((mask.value & (1 << layer)) > 0);
        }
         
		public static bool Contains(this LayerMask mask, GameObject gameobject) 
        {
             return ((mask.value & (1 << gameobject.layer)) > 0);
        }

		static List<Component> m_ComponentCache = new List<Component>();

		public static Component GetComponentNoAlloc(this GameObject @this, System.Type componentType) 
		{ 
			@this.GetComponents(componentType, m_ComponentCache); 
			var component = m_ComponentCache.Count > 0 ? m_ComponentCache[0] : null; 
			m_ComponentCache.Clear(); 
			return component; 
		} 

		public static T GetComponentNoAlloc<T>(this GameObject @this) where T : Component
		{
			@this.GetComponents(typeof(T), m_ComponentCache);
			var component = m_ComponentCache.Count > 0 ? m_ComponentCache[0] : null;
			m_ComponentCache.Clear();
			return component as T;
		} 
	}
}