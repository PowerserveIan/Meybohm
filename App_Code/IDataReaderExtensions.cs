/*
 * IDataReaderExtensions.cs
 * 
 * SecureGive
 * Powerserve 2013
 */

#region References

using System;
using System.Data;

#endregion

public static class IDataReaderExtensions
{
    #region Methods

    /// <summary>
    /// Checks if a column exists - allows stored procedures/calls to be modular.
    /// </summary>
    /// <param name="reader">The IDataReader with information.</param>
    /// <param name="fieldName">The field name to check the column for.</param>
    /// <returns></returns>
    public static bool FieldExists(this IDataReader reader, string fieldName)
    {
        reader.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", fieldName);
        return (reader.GetSchemaTable().DefaultView.Count > 0);
    }

    /// <summary>
    /// Gets the value with the given name.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="name">The value's name.</param>
    /// <returns>The value with the given name.</returns>
    public static object GetOrNull(this IDataReader reader, string name)
    {
        if (reader.FieldExists(name))
        {
            var value = reader[name];

            if (value == DBNull.Value)
            {
                return null;
            }

            return value;
        }

        return null;
    }

    /// <summary>
    /// Gets the value with the given name. (used for decimal/double)
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="name">The value's name.</param>
    /// <returns>The value with the given name.</returns>
    public static object GetOrZero(this IDataReader reader, string name)
    {
        if (reader.FieldExists(name))
        {
            var value = reader[name];

            if (value == DBNull.Value)
            {
                return 0;
            }

            return value;
        }

        return 0;
    }

    #endregion
}
