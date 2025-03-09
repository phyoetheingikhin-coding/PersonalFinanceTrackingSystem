using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace PersonalFinanceTrackingSystem.Shared.DapperService;

public class DapperService
{
    private readonly string _connectionString;
    public DapperService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<T> Query<T>(string query, object? param = null)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        List<T> lst = db.Query<T>(query, param).ToList();
        return lst;
    }
}