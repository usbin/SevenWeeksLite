using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
	//Resource.Load로 읽기:Resources 폴더에서 시작
	public static readonly string EVENT_BASE_PATH = "Event/";

	private List<GameEventListener> listeners =
		new List<GameEventListener>();

	public bool done = false;
	public void Raise()
	{
		done = true;
		for (int i = listeners.Count - 1; i >= 0; i--)
		{
			listeners[i].OnEventRaised();
		}
	}

	public void RegisterListener(GameEventListener listener)
	{ listeners.Add(listener); }

	public void UnregisterListener(GameEventListener listener)
	{ listeners.Remove(listener); }
}