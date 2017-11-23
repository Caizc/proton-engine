using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MoreMountains.Tools
{	
	/// <summary>
	/// A base class, meant to be extended depending on the use (simple, multiple object pooler), and used as an interface by the spawners.
	/// Still handles common stuff like singleton and initialization on start().
	/// DO NOT add this class to a prefab, nothing would happen. Instead, add SimpleObjectPooler or MultipleObjectPooler.
	/// </summary>
	public class ObjectPooler : MonoBehaviour
	{
	    public static ObjectPooler Instance;

		/// <summary>
		/// Singleton
		/// </summary>
	    protected virtual void Awake()
	    {
			Instance = this;
			FillObjectPool();
	    }

	    /// <summary>
	    /// On start, we fill the pool with the specified gameobjects
	    /// </summary>
	    protected virtual void Start()
	    {
			
	    }

		/// <summary>
		/// Implement this method to fill the pool with objects
		/// </summary>
	    protected virtual void FillObjectPool()
	    {
	        return ;
	    }

		/// <summary>
		/// Implement this method to return a gameobject
		/// </summary>
		/// <returns>The pooled game object.</returns>
		public virtual GameObject GetPooledGameObject()
	    {
	        return null;
	    }
	}
}