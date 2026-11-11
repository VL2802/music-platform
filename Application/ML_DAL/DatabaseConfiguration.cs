namespace ML_DAL
{

    public abstract class DatabaseConfiguration
    {
        protected readonly string _connectionString;

        protected DatabaseConfiguration(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
   
}
