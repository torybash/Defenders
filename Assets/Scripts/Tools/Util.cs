using System;
using System.Collections;
using System.Collections.Generic;
//using System.Collections.Concurrent;


public class Util {
}

public class ObjPool<T> where T : class
{
	Queue<T> queue = new Queue<T>();

	public T TryGet()
	{
		if (queue.Count > 0)
		{
			return queue.Dequeue();
		}

		return null;
	}

	public void Return(T obj)
	{
		if (obj != null)
		{
			queue.Enqueue(obj);
		}
	}

	protected virtual T Create(){return default(T);}
}


// Generic Object Pool Class
//public class ObjectPool<T>
//{
//	// ConcurrentBag used to store and retrieve objects from Pool.
//	private ConcurrentBag<T> _objects;
//	private Func<T> _objectGenerator;
//
//	// Object pool contructor used to get a delegate for implementing instance initialization
//	// or retrieval process
//	public ObjectPool(Func<T> objectGenerator)
//	{
//		if (objectGenerator == null) throw new ArgumentNullException("objectGenerator");
//		_objects = new ConcurrentBag<T>();
//		_objectGenerator = objectGenerator;
//	}
//
//	// GetObject retrieves the object from the object pool (if already exists) or else
//	// creates an instance of object and returns (if not exists)
//	public T GetObject()
//	{
//		T item;
//		if (_objects.TryTake(out item)) return item;
//		return _objectGenerator();
//	}
//
//	// PutObject stores back the object back to pool.
//	public void PutObject(T item)
//	{
//		_objects.Add(item);
//	}
//}
