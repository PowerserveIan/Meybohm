using System;
using System.Data;
using System.IO;
using System.Web;
using System.Collections.Generic;
using LINQtoCSV;
using DataRow = System.Data.DataRow;

namespace BaseCode
{
	/// <summary>
	/// Written By: Charles Cook
	/// Used to write CSV
	/// </summary>
	public class CSVWriteHelper
	{
		public static void WriteCSVToResponse<T>(IEnumerable<T> rows, bool writeHeaders, HttpResponse Response, string extensionlessFileName = "data")
		{
			ResponseHelpers(Response, extensionlessFileName);
			CsvContext cc = new CsvContext();
			cc.Write(rows, Response.Output, new CsvFileDescription { FirstLineHasColumnNames = writeHeaders });
			Response.End();
		}

		public static void WriteCSVToResponse(DataRowCollection rows, bool writeHeaders, HttpResponse Response, string extensionlessFileName = "data")
		{
			ResponseHelpers(Response, extensionlessFileName);

			WriteCSV(rows, Response.Output, writeHeaders);
			Response.End();
		}

		private static void ResponseHelpers(HttpResponse Response, string extensionlessFileName)
		{
			Response.Clear();
			Response.Buffer = true;
			Response.ContentType = "text/csv";
			Response.AddHeader("Content-Disposition", "attachment;filename=\"" + extensionlessFileName + ".csv\"");
			Response.Charset = string.Empty;
		}

		public static void WriteCSV(DataRowCollection rows, TextWriter writer, bool WriteHeaders)
		{
			if (WriteHeaders && rows.Count > 0)
			{
				foreach (DataColumn col in rows[0].Table.Columns)
				{
					writer.Write(PrepareField(col.ColumnName));
					if (col != rows[0].Table.Columns[rows[0].Table.Columns.Count - 1])
						writer.Write(",");
				}
				writer.Write("\n");
			}

			foreach (DataRow dr in rows)
			{
				for (int i = 0; i < dr.Table.Columns.Count; i++)
				{
					writer.Write(PrepareField(dr[i]));
					if (i != dr.Table.Columns.Count - 1)
						writer.Write(",");
				}
				writer.Write("\n");
			}
		}

		private static string PrepareField(object value)
		{
			return String.Format("\"{0}\"", Convert.ToString(value).Replace("\"", "\"\""));
		}
	}
}