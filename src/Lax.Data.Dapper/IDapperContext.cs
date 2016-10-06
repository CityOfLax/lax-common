using System.Data.SqlClient;

namespace Lax.Data.Dapper {

    public interface IDapperContext {

        SqlConnection GetSqlConnection();

    }

}
