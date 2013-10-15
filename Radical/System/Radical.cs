using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

public class Lookup<TK, TR> : Dictionary<TK, TR> where TR : class
{
	public new virtual TR this[TK index]
	{
		get
		{
			if (ContainsKey(index))
			{
				return base[index];
			}
			return null;
			
		}
		set
		{
			base[index] = value;
		}
	}
	
	public T Get<T>(TK index) where T : class
	{
		return this[index] as T;
	}
	
	
	
}

public interface IChanged
{
	void Changed(object index);
}
public interface INeedParent
{
	void SetParent(IChanged parent, object index);
}
	
public class Index<TK, TR> : Lookup<TK,TR>, IChanged  where TR : class, new()
{

	public event Action<TK,TR, TR> Setting;
	public event Action<TK,TR> Getting = delegate {};
	
	public void Changed(object index)
	{
		if (Setting != null)
		{
			TR current = null;
			if (base.ContainsKey((TK)index))
			{
				current = base[(TK)index];
			}
			Setting((TK)index, current, current);
		}
	}
	
	public override TR this[TK index]
	{
		get
		{
			if (ContainsKey(index))
			{
				return base[index];
			}
			var ret = new TR();
			if (ret is INeedParent)
			{
				(ret as INeedParent).SetParent(this,index);
			}
			base[index] = ret;
			Getting(index, ret);
			return ret;
		}
		set
		{
			if (Setting != null)
			{
				TR current = null;
				if (base.ContainsKey(index))
				{
					current = base[index];
				}
				Setting(index, current, value);
			}
			base[index] = value;
		}
	}
}





public static class Radical
{

	private static int _indent=0;
	public static readonly bool DebugBuild;
	public static int _deferredLoggingEnabled = 0;
    public static bool AllowDeferredLogging = false;
	
	public class Logging : IDisposable
	{
		public Logging()
		{
			
			_deferredLoggingEnabled++;
		}
		
		public void Dispose()
		{
			_deferredLoggingEnabled--;
			if (_deferredLoggingEnabled == 0)
			{
				Radical.CommitLog();
			}
		}
	}
	
	public static bool DeferredLoggingEnabled
	{
		get
		{
			return _deferredLoggingEnabled > 0;
		}
	}
	
		
	static Radical()
	{
		DebugBuild = UnityEngine.Debug.isDebugBuild;
	}
	
	
	public static void IndentLog()
	{
		_indent++;
	}
	
	public static void OutdentLog()
	{
		_indent--;
	}
	
	private static List<string> logEntries = new List<string>();
	
	public static void LogNode(object message)
	{
		LogNow(message.ToString());
	}
	public static void LogNow(string message, params object[] parms)
	{
		if (!DebugBuild)
			return;
		UnityEngine.Debug.Log(string.Format(message, parms));
	}
	
	public static void LogWarning(string message)
	{
		LogWarning ( message, null);
	}
	public static void LogWarning(string message, UnityEngine.Object context)
	{
		if (!DebugBuild)
			return;
		if (context != null)
		{
			UnityEngine.Debug.LogWarning(message, context);
		}
		else
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}
	
	public static void LogError(string message)
	{
		LogError( message, null);
	}
	public static void LogError(string message, UnityEngine.Object context)
	{
		if (!DebugBuild)
		{
			return;
		}
		if (context != null)
			UnityEngine.Debug.LogError(message, context);
		else
			UnityEngine.Debug.LogError(message);
	}
	
	public static bool IsLogging()
	{
		if (DebugBuild == false || ! DeferredLoggingEnabled)
		{
			return false;
		}
		return true;
		
	}
	
	public static void Log(string message, params object[] parms)
	{
		if (DebugBuild == false || ! DeferredLoggingEnabled || !AllowDeferredLogging)
		{
			return;
		}
		logEntries.Add((new string(' ', 4 * _indent)) + string.Format(message, parms));
		if (logEntries.Count > 50000)
		{
			logEntries.RemoveAt(0);
		}
	}
	
	public static void ClearLog()
	{
		logEntries.Clear();
	}
	
	public static void CommitLog()
	{
		if (logEntries.Count == 0)
		{
			return;
		}
		var sb = logEntries.Aggregate((current, next) => current + "\n" + next);
		UnityEngine.Debug.Log(sb);
		logEntries.Clear();
	}
	
}


