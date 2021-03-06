using UnityEngine;
using System.Collections;
using System;

public enum CoroutineState
{
	Ready,
	Running,
	Paused,
	Finished
}

public class Extensions : ScriptableObject {

	//newword
	public static Texture2D textureFromSprite(Sprite sprite)
	{
		if(sprite.rect.width != sprite.texture.width){
			Texture2D newText = new Texture2D((int)sprite.rect.width,(int)sprite.rect.height);
			Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x, 
				(int)sprite.textureRect.y, 
				(int)sprite.textureRect.width, 
				(int)sprite.textureRect.height );
			newText.SetPixels(newColors);
			newText.Apply();
			return newText;
		} else
			return sprite.texture;
	}


	//thisisnew
}
#region CoroutineExtensions
public static class CoroutineExtensions
{
	public static Coroutine StartCoroutineEx(this MonoBehaviour monoBehaviour, IEnumerator routine, out CoroutineController coroutineController)
	{
		if (routine == null)
		{
			throw new System.ArgumentNullException("routine");
		}

		coroutineController = new CoroutineController(routine);
		return monoBehaviour.StartCoroutine(coroutineController.Start());
	}
}
public class CoroutineController
{
	private IEnumerator _routine;

	public CoroutineState state;

	public CoroutineController(IEnumerator routine)
	{
		_routine = routine;
		state = CoroutineState.Ready;
	}

	public IEnumerator Start()
	{
		if (state != CoroutineState.Ready)
		{
			throw new System.InvalidOperationException("Unable to start coroutine in state: " + state);
		}

		state = CoroutineState.Running;
		while (_routine.MoveNext())
		{
			yield return _routine.Current;
			while (state == CoroutineState.Paused)
			{
				yield return null;
			}
			if (state == CoroutineState.Finished)
			{
				yield break;
			}
		}

		state = CoroutineState.Finished;
	}

	public void Stop()
	{
		if (state != CoroutineState.Running && state != CoroutineState.Paused)
		{
			throw new System.InvalidOperationException("Unable to stop coroutine in state: " + state);
		}

		state = CoroutineState.Finished;
	}

	public void Pause()
	{
		if (state != CoroutineState.Running)
		{
			throw new System.InvalidOperationException("Unable to pause coroutine in state: " + state);
		}

		state = CoroutineState.Paused;
	}

	public void Resume()
	{
		if (state != CoroutineState.Paused)
		{
			throw new System.InvalidOperationException("Unable to resume coroutine in state: " + state);
		}

		state = CoroutineState.Running;
	}

}
#endregion
#region AnimatorExtensions	
public static class Extensions_Animator{
public static bool HasParameter(this Animator animator, string paramName)
{
	foreach (AnimatorControllerParameter param in animator.parameters)
	{
		if (param.name == paramName) 
			return true;
	}
	return false;
}
public static bool HasParameter(this Animator animator, int id)
{
        if (!animator.gameObject.activeInHierarchy)
            return false;

	foreach (AnimatorControllerParameter param in animator.parameters)
	{
		if (param.GetHashCode () == id) 
			return true;
	}
	return false;
}


}
#endregion
#region TransformExtensions

public static class TransformDeepChildExtension
{
	//Breadth-first search
	public static Transform FindDeepChild(this Transform aParent, string aName)
	{
		var result = aParent.Find(aName);
		if (result != null)
			return result;
		foreach(Transform child in aParent)
		{
			result = child.FindDeepChild(aName);
			if (result != null)
				return result;
		}
		return null;
	}
}
#endregion
#region JSONExtensions
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
#endregion