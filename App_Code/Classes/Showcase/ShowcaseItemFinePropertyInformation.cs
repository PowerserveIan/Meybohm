/**
 * ShowcaseItemFinePropertyInformation.cs
 * Copyright (c) Powerserve 2013. All rights reserved.
 * */

#region References

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Classes.SEOComponent;
using System.Configuration;
using System.Data.SqlClient;

#endregion

/// <summary>
/// The showcase item fine property information (description, isFine, etc.)
/// </summary>
public class ShowcaseItemFinePropertyInformation
{
    #region Constructors

    public ShowcaseItemFinePropertyInformation()
	{

    }

    /// <summary>
    /// Initializes an object for showcase item fine property
    /// </summary>
    /// <param name="reader"></param>
    public ShowcaseItemFinePropertyInformation(IDataReader reader)
    {
        this.ShowcaseItemFinePropertyInformationId = (int)reader.GetOrNull("ShowcaseItemFinePropertyInformationId");
        this.ShowcaseItemId = (int)reader.GetOrNull("ShowcaseItemId");
        this.Description = (string)reader.GetOrNull("Description");
        this.Features = (string)reader.GetOrNull("Features");
        this.Tags = (string)reader.GetOrNull("Tags");
        this.IsFine = (bool)reader.GetOrNull("IsFine");
        this.IsFineFeatured = (bool)reader.GetOrNull("IsFineFeatured");
    }

    #endregion

    #region Properties

    /// <summary>
    /// The Id of the property information.
    /// </summary>
    public int ShowcaseItemFinePropertyInformationId { get; set; }

    /// <summary>
    /// The Id of the ShowcaseItem.
    /// </summary>
    public int ShowcaseItemId { get; set; }

    /// <summary>
    /// The description of the fine showcase item.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The features associated with the features.
    /// </summary>
    public string Features { get; set; }
        
    /// <summary>
    /// Determines if the property is fine or not.
    /// </summary>
    public bool IsFine { get; set; }
       
    /// <summary>
    /// Determines if the property is featured or not.
    /// </summary>
    public bool IsFineFeatured { get; set; }
       
    /// <summary>
    /// The Tags associated with the fine property.
    /// </summary>
    public string Tags { get; set; }

    #endregion

    #region Methods

    /// <summary>
    /// Adds a new showcase item fine property information
    /// </summary>
    private void Add()
    {
        // Initialize SP and list parameters.
        var cmdText = "ps_ShowcaseItemFinePropertyInformation_Add";
        var connectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

        // Set up connection/command.
        using (var connection = new SqlConnection(connectionString))
        {
            using (var command = new SqlCommand(cmdText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ShowcaseItemId", this.ShowcaseItemId);
                command.Parameters.AddWithValue("@Description", this.Description);
                command.Parameters.AddWithValue("@Features", this.Features);
                command.Parameters.AddWithValue("@IsFine", this.IsFine);
                command.Parameters.AddWithValue("@IsFineFeatured", this.IsFineFeatured);
                command.Parameters.AddWithValue("@Tags", this.Tags);

                // Open the connection, read the data (if any), and return the object.
                connection.Open();
                this.ShowcaseItemFinePropertyInformationId = Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }

    /// <summary>
    /// Gets the showcase item property information from a showcase Id
    /// </summary>
    /// <param name="showcaseItemId">The Id of the Showcase Item (Property)</param>
    /// <returns></returns>
    public static ShowcaseItemFinePropertyInformation Get(int showcaseItemId)
    {
        // Initialize SP and list parameters.
        var cmdText = "ps_ShowcaseItemFinePropertyInformation_Get";
        var connectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

        // Set up connection/command.
        using (var connection = new SqlConnection(connectionString))
        {
            using (var command = new SqlCommand(cmdText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ShowcaseItemID", showcaseItemId);

                // Open the connection, read the data (if any), and return the object.
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ShowcaseItemFinePropertyInformation(reader);
                    }
                    else
                    {
                        return new ShowcaseItemFinePropertyInformation();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Saves the showcase item fine property information.
    /// </summary>
    public void Save()
    {
        if (this.ShowcaseItemFinePropertyInformationId == 0)
        {
            this.Add();
        }
        else
        {
            this.Update();
        }
    }

    /// <summary>
    /// Updates an existing showcase item fine property
    /// </summary>
    private void Update()
    {
        // Initialize SP and list parameters.
        var cmdText = "ps_ShowcaseItemFinePropertyInformation_Update";
        var connectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

        // Set up connection/command.
        using (var connection = new SqlConnection(connectionString))
        {
            using (var command = new SqlCommand(cmdText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ShowcaseItemFinePropertyInformationId", this.ShowcaseItemFinePropertyInformationId);
                command.Parameters.AddWithValue("@ShowcaseItemId", this.ShowcaseItemId);
                command.Parameters.AddWithValue("@Description", this.Description);
                command.Parameters.AddWithValue("@Features", this.Features);
                command.Parameters.AddWithValue("@IsFine", this.IsFine);
                command.Parameters.AddWithValue("@IsFineFeatured", this.IsFineFeatured);
                command.Parameters.AddWithValue("@Tags", this.Tags);

                // Open the connection, read the data (if any), and return the object.
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

    #endregion
}