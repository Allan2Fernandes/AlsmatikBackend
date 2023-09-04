
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;


namespace AlsmatikBackend
{
    public class DbHandler
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        SqlConnection connection;
        //Use the private constructor because singleton
        private static DbHandler Instance = new DbHandler();
        private DbHandler() // singleton constructor
        {
            //Get the environment variable
            //string envVariable = Environment.GetEnvironmentVariable("SQLPDHSDetails", EnvironmentVariableTarget.Machine).ToString();
            string envVariable = Environment.GetEnvironmentVariable("DatabaseConnection", EnvironmentVariableTarget.Machine).ToString();
            //The single variable holds all the information, so split it up
            var variables = envVariable.Split(',');

            //Use the list to get individual variables
            builder.DataSource = variables[0].ToString();
            builder.UserID = variables[1].ToString();
            builder.Password = variables[2].ToString();
            builder.InitialCatalog = variables[3].ToString();
            builder.MultipleActiveResultSets = true;
            //connectToDB();
        }

        public static DbHandler GetDbHandlerInstance() //Singleton getter
        {
            return Instance;
        }

        public void connectToDB()
        {
            connection = new SqlConnection(builder.ConnectionString);
            connection.Open();
        }

        public List<List<Dictionary<string, object>>> ExecuteRawQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Close any existing DataReader if it is open
                    if (command.Connection.State == ConnectionState.Open)
                    {
                        command.Connection.Close();
                    }

                    command.Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        var allData = new List<List<Dictionary<string, object>>>();

                        do
                        {
                            var DataFromSelectStatement = new List<Dictionary<string, object>>();
                            while (reader.Read())
                            {
                                var allFieldNames = new List<string>();
                                var RowElement = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    var fieldName = reader.GetName(i);
                                    allFieldNames.Add(fieldName);
                                    RowElement[fieldName] = (reader[fieldName].ToString().Length == 0 ? "" : reader[fieldName]);
                                }

                                DataFromSelectStatement.Add(RowElement);
                            }
                            allData.Add(DataFromSelectStatement);
                        } while (reader.NextResult());
                        return allData.ToList();
                    }
                }
            }            
        }

       
    }
}
