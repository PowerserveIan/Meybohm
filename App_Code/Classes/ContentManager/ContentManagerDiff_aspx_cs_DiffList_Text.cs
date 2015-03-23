using System;
using System.Collections;
using DifferenceEngine;

namespace Classes.ContentManager
{
	public class DiffList_Text : IDiffList
	{
		private readonly ArrayList _lines;

		public DiffList_Text(string content)
		{
			_lines = new ArrayList();
			foreach (string line in content.Split('\n'))
			{
				_lines.Add(line);
			}
		}

		#region IDiffList Members

		public int Count()
		{
			return _lines.Count;
		}

		public IComparable GetByIndex(int index)
		{
			return (string)_lines[index];
		}

		#endregion
	}
}