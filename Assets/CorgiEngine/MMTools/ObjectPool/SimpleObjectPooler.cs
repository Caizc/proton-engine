using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MoreMountains.Tools
{	

	public class SimpleObjectPooler : ObjectPooler 
	{
	    /// the game object we'll instantiate 
		public GameObject GameObjectToPool;
	    /// the number of objects we'll add to the pool
		public int PoolSize = 20;
	    /// if true, the pool will automatically add objects to the itself if needed
		public bool PoolCanExpand = true;

	    /// this object is just used to group the pooled objects
	    protected GameObject _waitingPool;
	    /// the actual object pool
		protected List<GameObject> _pooledGameObjects;
	    
	    /// <summary>
	    /// Fills the object pool with the gameobject type you've specified in the inspector
	    /// </summary>
	    protected override void FillObjectPool()
	    {
	        // we create a container that will hold all the instances we create
	        _waitingPool = new GameObject("[SimpleObjectPooler] " + this.name);

			// we initialize the list we'll use to 
			_pooledGameObjects = new List<GameObject>();

			// we add to the pool the specified number of objects
	        for (int i = 0; i < PoolSize; i++)
	        {
	            AddOneObjectToThePool ();
	        }
	    }
	    	
	    /// <summary>
	    /// This method returns one inactive object from the pool
	    /// </summary>
	    /// <returns>The pooled game object.</returns>
		public override GameObject GetPooledGameObject()
		{
			// we go through the pool looking for an inactive object
			for (int i=0; i< _pooledGameObjects.Count; i++)
			{
				if (!_pooledGameObjects[i].gameObject.activeInHierarchy)
	            {
	            	// if we find one, we return it
	                return _pooledGameObjects[i];
				}
			}
			// if we haven't found an inactive object (the pool is empty), and if we can extend it, we add one new object to the pool, and return it		
			if (PoolCanExpand)
			{
				return AddOneObjectToThePool();
			}
			// if the pool is empty and can't grow, we return nothing.
			return null;
		}
		
		/// <summary>
		/// Adds one object of the specified type (in the inspector) to the pool.
		/// </summary>
		/// <returns>The one object to the pool.</returns>
		protected virtual GameObject AddOneObjectToThePool()
		{
			if (GameObjectToPool==null)
			{
				Debug.LogWarning("The "+gameObject.name+" ObjectPooler doesn't have any GameObjectToPool defined.", gameObject);
				return null;
			}
			GameObject newGameObject = (GameObject)Instantiate(GameObjectToPool);
			newGameObject.gameObject.SetActive(false);
			newGameObject.transform.SetParent(_waitingPool.transform);
			newGameObject.name=GameObjectToPool.name+"-"+_pooledGameObjects.Count;
			_pooledGameObjects.Add(newGameObject);
			return newGameObject;
		}
	}
}