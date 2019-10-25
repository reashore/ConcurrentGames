using Npgsql;
using System;
using log4net;

namespace SportsIq.SqlDataAccess
{
    public static class PostgresDataReaderExtensions
    {
        private static readonly ILog Logger;

        static PostgresDataReaderExtensions()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(PostgresDataReaderExtensions));
        }
        
        #region Accessors for standard types

        public static string GetString(this NpgsqlDataReader mySqlDataReader, string columnName)
        {
            int columnIndex = mySqlDataReader.GetOrdinal(columnName);
            string value = mySqlDataReader.GetString(columnIndex);

            return value;
        }

        public static int GetInt(this NpgsqlDataReader mySqlDataReader, string columnName)
        {
            int columnIndex = mySqlDataReader.GetOrdinal(columnName);
            int value = mySqlDataReader.GetInt32(columnIndex);

            return value;
        }

        public static double GetDouble(this NpgsqlDataReader mySqlDataReader, string columnName)
        {
            int columnIndex = mySqlDataReader.GetOrdinal(columnName);
            double value = mySqlDataReader.GetDouble(columnIndex);

            return value;
        }

        public static Guid GetGuid(this NpgsqlDataReader mySqlDataReader, string columnName)
        {
            int columnIndex = mySqlDataReader.GetOrdinal(columnName);
            Guid value = mySqlDataReader.GetGuid(columnIndex);

            return value;
        }

        public static DateTime GetDateTime(this NpgsqlDataReader mySqlDataReader, string columnName)
        {
            int columnIndex = mySqlDataReader.GetOrdinal(columnName);
            DateTime value = mySqlDataReader.GetDateTime(columnIndex);

            return value;
        }

        #endregion

        #region Accessors for nullable types

        public static int? GetNullableInt(this NpgsqlDataReader mySqlDataReader, string columnName)
        {
            int columnIndex = mySqlDataReader.GetOrdinal(columnName);

            if (mySqlDataReader.IsDBNull(columnIndex))
            {
                return null;
            }

            int intValue;

            try
            {
                intValue = mySqlDataReader.GetInt32(columnIndex);
            }
            catch (Exception)
            {
                Logger.Error("Could not parse int, returning null");
                return null;
            }

            return intValue;
        }

        public static double? GetNullableDouble(this NpgsqlDataReader mySqlDataReader, string columnName)
        {
            int columnIndex = mySqlDataReader.GetOrdinal(columnName);

            if (mySqlDataReader.IsDBNull(columnIndex))
            {
                return null;
            }

            double doubleValue;

            try
            {
                doubleValue = mySqlDataReader.GetDouble(columnIndex);
            }
            catch (Exception)
            {
                Logger.Error("Could not parse double, returning null");
                return null;
            }

            return doubleValue;
        }

        public static string GetNullableString(this NpgsqlDataReader mySqlDataReader, string columnName)
        {
            int columnIndex = mySqlDataReader.GetOrdinal(columnName);

            if (mySqlDataReader.IsDBNull(columnIndex))
            {
                return null;
            }

            string stringValue;

            try
            {
                stringValue = mySqlDataReader.GetString(columnIndex);
            }
            catch (Exception)
            {
                Logger.Error("Could not parse string, returning null");
                return null;
            }

            return stringValue;
        }

        public static DateTime? GetNullableDateTime(this NpgsqlDataReader mySqlDataReader, string columnName)
        {
            int columnIndex = mySqlDataReader.GetOrdinal(columnName);

            if (mySqlDataReader.IsDBNull(columnIndex))
            {
                return null;
            }

            DateTime dateTime;

            try
            {
                dateTime = mySqlDataReader.GetDateTime(columnIndex);
            }
            catch (Exception)
            {
                Logger.Error("Could not parse DateTime, returning null");
                return null;
            }

            return dateTime;
        }

        public static Guid? GetNullableGuid(this NpgsqlDataReader mySqlDataReader, string columnName)
        {
            int columnIndex = mySqlDataReader.GetOrdinal(columnName);

            if (mySqlDataReader.IsDBNull(columnIndex))
            {
                return null;
            }

            Guid guid;

            try
            {
                guid = mySqlDataReader.GetGuid(columnIndex);
            }
            catch (Exception)
            {
                Logger.Error("Could not parse Guid, returning null");
                return null;
            }

            return guid;
        }

        #endregion
    }
}