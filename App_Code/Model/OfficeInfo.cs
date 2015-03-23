/*
 * OfficeInfo.cs
 * 
 * Meybohm
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace Powerserve.Meybohm.Model
{
    /// <summary>
    /// Represents information about an office and its office manager.
    /// </summary>
    public class OfficeInfo
    {
        /// <summary>
        /// Creates a new instance of the OfficeInfo class.
        /// </summary>
        /// <param name="data">The office info's data.</param>
        private OfficeInfo(IDataReader data)
        {
            if (data != null)
            {
                this.ManagerFirstName = (string)(data["ManagerFirstName"] != DBNull.Value ? data["ManagerFirstName"] : null);
                this.ManagerImage = (string)(data["ManagerImage"] != DBNull.Value ? data["ManagerImage"] : null);
                this.ManagerLastName = (string)(data["ManagerLastName"] != DBNull.Value ? data["ManagerLastName"] : null);
                this.OfficeAddress = (string)data["OfficeAddress"];
                this.OfficeCity = (string)data["OfficeCity"];
                this.OfficeFax = (string)data["OfficeFax"];
                this.OfficeImage = (string)data["OfficeImage"];
                this.OfficeLatitude = (decimal)data["OfficeLatitude"];
                this.OfficeLongitude = (decimal)data["OfficeLongitude"];
                this.OfficeName = (string)data["OfficeName"];
                this.OfficePhone = (string)data["OfficePhone"];
                this.OfficeState = (string)data["OfficeState"];
                this.OfficeZip = (string)data["OfficeZip"];
            }
        }

        public string ManagerFirstName { get; private set; }
        public string ManagerImage { get; private set; }
        public string ManagerLastName { get; private set; }
        public string OfficeAddress { get; private set; }
        public string OfficeCity { get; private set; }
        public string OfficeFax { get; private set; }
        public string OfficeImage { get; private set; }
        public decimal OfficeLatitude { get; private set; }
        public decimal OfficeLongitude { get; private set; }
        public string OfficeName { get; private set; }
        public string OfficePhone { get; private set; }
        public string OfficeState { get; private set; }
        public string OfficeZip { get; private set; }

        /// <summary>
        /// Gets Meybohm's active offices that have images sorted by name.
        /// </summary>
        /// <returns>Meybohm's active offices that have images sorted by name.</returns>
        public static List<OfficeInfo> GetList()
        {
            List<OfficeInfo> offices = new List<OfficeInfo>();

            string cmdText = @"
            SELECT
	            UserInfo.FirstName  AS ManagerFirstName,
	            UserInfo.Photo      AS ManagerImage,
	            UserInfo.LastName   AS ManagerLastName,
	            [Address].Address1  AS OfficeAddress,
	            [Address].City      AS OfficeCity,
	            Office.Fax          AS OfficeFax,
	            Office.[Image]      AS OfficeImage,
                [Address].Latitude  AS OfficeLatitude,
                [Address].Longitude AS OfficeLongitude,
	            Office.Name         AS OfficeName,
	            Office.Phone        AS OfficePhone,
	            [State].Name        AS OfficeState,
	            [Address].Zip       AS OfficeZip
            FROM
	            Office
	            JOIN      [Address]     ON Office.AddressID = [Address].AddressID
	            JOIN      [State]       ON [Address].StateID = [State].StateID
	            LEFT JOIN OfficeManager ON Office.OfficeID = OfficeManager.OfficeId
	            LEFT JOIN UserInfo      ON OfficeManager.UserInfoId = UserInfo.UserInfoID
            WHERE
	            Office.Active = 1 AND
	            Office.IsMeybohm = 1 AND
	            Office.[Image] IS NOT NULL
            ORDER BY
                Office.Name";

            string connectionString = WebConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(cmdText, connection))
            {
                connection.Open();

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        OfficeInfo officeInfo = new OfficeInfo(reader);
                        offices.Add(officeInfo);
                    }
                }
            }

            return offices;
        }
    }
}