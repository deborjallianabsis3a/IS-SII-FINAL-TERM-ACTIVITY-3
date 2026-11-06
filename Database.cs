using System.Data.OleDb;

namespace DeBorja_
{
    public static class Database
    {
        private static string connectionString =
            @"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\SR-20\OneDrive\Documents\InventoryDb.accdb;";

        public static OleDbConnection GetConnection()
        {
            return new OleDbConnection(connectionString);
        }
    }
}