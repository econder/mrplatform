/***************************************************************************************************
 * Class:    	MREventLog.cs
 * Created By:  Eric Conder
 * Created On:  2014-01-10
 * 
 * Changes:
 * 
 * 2014-02-28	Added documentation to the class constructor, properties, functions, & methods.
 *
 * 2014-04-01	Changed namespace from MRPlatform2014.Event to MRPlatform2014.AlarmEvent
 * *************************************************************************************************/
using System;
using System.Diagnostics;
using System.Reflection;

namespace MRPlatform2014.AlarmEvent
{
	/// <summary>
	/// Description of WinEventLog.
	/// </summary>
	public class WinEventLog
	{
		public WinEventLog()
		{
			//EventSourceCreationData escd = new EventSourceCreationData("MRPlatform2014", "MRPlatform");
			//EventLog.CreateEventSource(escd);
		}
		
		private const string APPSOURCE = "MRPlatform2014";
		private const string LOGDEST = "MRPlatform";
		
		public void WriteEvent(string message)
		{
			/*
			EventLogPermission elPerm = new EventLogPermission(EventLogPermissionAccess.Administer, ".");
			elPerm.PermitOnly();
			
			EventLog el = new EventLog();
			
			if(!EventLog.SourceExists(APPSOURCE, "."))
			{
				EventLog.CreateEventSource(APPSOURCE, LOGDEST);
			}
			
			el.Source = APPSOURCE;
			el.EnableRaisingEvents = true;
			el.WriteEntry(message);
			*/
			
			return;
		}
	}
}
