using System.Data.SqlClient;

namespace AlsmatikBackend
{
    public class DbHandler
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        SqlConnection connection;
        private static DbHandler instance = new DbHandler();
        private DbHandler() // singleton constructor
        {
            builder.DataSource = "DESKTOP-2B3AJNE";
            builder.UserID = "sa";
            builder.Password = "123456";
            builder.InitialCatalog = "PDHS_Guide";
            connectToDB();
        }

        public static DbHandler GetDbHandlerInstance() //Singleton getter
        {
            return instance;
        }

        public void connectToDB()
        {
            connection = new SqlConnection(builder.ConnectionString);
            connection.Open();
        }

        public List<object> GetChosenParams(int UserID, int ArtGrp, int ActualSetID) //2050, 3, 0
        {
            string sql = $"[dbo].[sp_GetFormParamUser] @UserID = {UserID},@ArtGrp = {ArtGrp}, @ActualSetID = {ActualSetID}";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            var allData = new List<object>();
            while (reader.Read())
            {
                var RowData = new
                {
                    ParamGroup = reader["ParamGroup"],
                    ParamID = reader["ParamID"],
                    ParText = reader["ParText"],
                    ParUnit = reader["ParUnit"],
                    ParComment = reader["ParComment"],
                    ParValue = reader["ParValue"],
                    Permission = reader["Permission"]
                };
                allData.Add(RowData);
            }
            reader.Close();
            return allData;
        }
    }
}
