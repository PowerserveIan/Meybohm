using System;
using System.Collections.Generic;

namespace Classes.Rets
{
	public partial class RetsTask
	{
		public bool LastRunSuccessful
		{
			get
			{
				bool temp = true;
				if (ParentRetsTaskID == null)
				{
					List<RetsTask> childrenTasks = RetsTask.RetsTaskGetByParentRetsTaskID(RetsTaskID);
					foreach (RetsTask childrenTask in childrenTasks)
					{
						RetsTaskStatus currentStatus =
							Rets.RetsTaskStatus.RetsTaskStatusGetByRetsTaskID(childrenTask.RetsTaskID).FindLast(r => r.RetsTaskID > 0);
						if (currentStatus != null && currentStatus.RetsStatusID > 1) temp = false;
					}
				}
				else
				{
					RetsTaskStatus currentStatus =
						Rets.RetsTaskStatus.RetsTaskStatusGetByRetsTaskID(RetsTaskID).FindLast(r => r.RetsTaskID > 0);
					if (currentStatus != null && currentStatus.RetsStatusID > 1) temp = false;
				}
				return temp;
			}
		}
		public DateTime? LastSuccessfulTime
		{
			get
			{
				DateTime? temp = null;
				if (ParentRetsTaskID != null)
				{
					RetsTaskStatus currentStatus =
						Rets.RetsTaskStatus.RetsTaskStatusGetByRetsTaskID(RetsTaskID).FindLast(r => r.RetsStatusID==1);
					if (currentStatus != null) temp = currentStatus.TaskCompleteTimeClientTime;
				}
				return temp;
			}
		}
		public DateTime? LastAttemptTime
		{
			get
			{
				DateTime? temp = null;
				if (ParentRetsTaskID != null)
				{
					RetsTaskStatus currentStatus =
						Rets.RetsTaskStatus.RetsTaskStatusGetByRetsTaskID(RetsTaskID).FindLast(r => r.RetsStatusID >0);
					if (currentStatus != null) temp = currentStatus.TaskCompleteTimeClientTime;
				}
				return temp;
			}
		}
	}
}