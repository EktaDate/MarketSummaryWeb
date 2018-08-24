using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MarketSummaryWeb.Repository
{
    public static class FactoryClass 
    {
        static DBType dbType = setDBType();

        private static DBType setDBType()
        {
            string dbtype = ConfigurationManager.AppSettings["dbtype"];
            if (dbtype == DBType.COSMOSDB.ToString())
            {
                return DBType.COSMOSDB;
            }
            else if(dbtype == DBType.TABLESTORAGE.ToString())
            {
                return DBType.TABLESTORAGE;
            }
            return DBType.AZURESQL;
        }
        public static IDBRepository CreateDBRepositoryObject()
        {
            IDBRepository objDBType = null;            
            switch (dbType)
            {
                case DBType.AZURESQL:

                    objDBType = new SQLRepository();
                    return objDBType;

                case DBType.COSMOSDB:

                    objDBType = new CosmosRepository();
                    return objDBType;

                case DBType.TABLESTORAGE:

                    objDBType = new TableStorageRepository();
                    return objDBType;

                default:
                    return null;

            }
        }
        

    }
    public enum DBType
    {
        AZURESQL,
        COSMOSDB,
        TABLESTORAGE
    }
}