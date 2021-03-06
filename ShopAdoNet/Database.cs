﻿using System.Data;
using System.Data.SqlClient;

namespace ShopAdoNet
{
    internal static class Database
    {
        internal static int ExecuteNonQuery(SqlCommand command, string connectionStringName)
        {
            using (var connection = new SqlConnection(connectionStringName))
            {
                connection.Open();

                using (command)
                {
                    command.Connection = connection;
                    return command.ExecuteNonQuery();
                }
            }
        }

        internal static object ExecuteScalar(SqlCommand command, string connectionStringName)
        {
            using (var connection = new SqlConnection(connectionStringName))
            {
                connection.Open();

                using (command)
                {
                    command.Connection = connection;
                    return command.ExecuteScalar();
                }
            }
        }

        internal static DataTable ExecuteDataTable(string cmdText, string connectionStringName)
        {
            using (var connection = new SqlConnection(connectionStringName))
            {
                connection.Open();

                using (var sqlDataAdapter = new SqlDataAdapter(cmdText, connection))
                {
                    var result = new DataSet();
                    sqlDataAdapter.Fill(result);
                    return result.Tables[0];
                }
            }
        }
    }
}