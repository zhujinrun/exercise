using AbstractDataBase.Data;
using AbstractDataBase.Data.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;
using 改进后的DataBase;

namespace DataaBase.DataTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestSQLReturnMultiDataTables()
        {
            string sql = "select * from Region;select * from Shippers";
            string[] tableName = new string[] { "Region", "Shippers" };

            DataSet result = SqlDbHelper.ExecuteDataSet(sql, tableName);
        }
        [TestMethod]
        public void TestParameterizedSQLReturnMultiDataTables()
        {
            string sql = "select * from Region where RegionID=@RegionID;";
            sql += "select * from Shippers where ShipperID=@ShipperID;";
            string[] tableName = new string[] { "Region", "Shipper" };
            SqlParameter regionID = new SqlParameter("@RegionID", 1);
            SqlParameter shipperID = new SqlParameter("@Shipper", 2);
            DataSet result = SqlDbHelper.ExecuteDataSet(sql, tableName, regionID, shipperID);
        }

        [TestMethod]
        public void Test()
        {
            DataBase db = DataBaseFactory.Create("LocalSQL");
            DataSet dataSet = db.ExecuteDataSet("select * from Shippers");
        }
        [TestMethod]
        public void TestNew()
        {
            DataBase db = DataBaseFactoryNew.Create("LocalSQL");
            DataSet dataSet = db.ExecuteDataSet("select * from Shippers");
        }
    }
}
