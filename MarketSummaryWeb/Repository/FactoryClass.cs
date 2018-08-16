using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MarketSummaryWeb.Repository
{
    public static class FactoryClass<T> where T : class
    {
        static DBType dbType = setDBType();

        private static DBType setDBType()
        {
            string dbtype = ConfigurationManager.AppSettings["dbtype"];
            if (dbtype == DBType.COSMOSDB.ToString())
            {
                return DBType.COSMOSDB;
            }
            return DBType.AZURESQL;
        }
        public static IDBRepository<T> CreateDBRepositoryObject()
        {
            IDBRepository<T> objDBType = null;            
            switch (dbType)
            {
                case DBType.AZURESQL:

                    objDBType = new SQLRepository<T>();
                    return objDBType;

                case DBType.COSMOSDB:

                    objDBType = new CosmosRepository<T>();
                    return objDBType;

                default:
                    return null;

            }
        }
        

    }
    public enum DBType
    {
        AZURESQL,
        COSMOSDB
    }
}