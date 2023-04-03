using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ShoppingCart.DB;

public class DbInterceptor : DbCommandInterceptor
{
    private readonly int maxRetries = 5;
    private readonly TimeSpan delay = TimeSpan.FromSeconds(30);

    public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        int retryCount = 0;
        while (true)
        {
            try
            {
                return result;
            }
            catch (SqlException ex) when (ex.Number == 53 && retryCount < maxRetries)
            {
                // 重試延遲時間
                await Task.Delay(delay, cancellationToken);

                // 重試次數 +1
                retryCount++;

                // 重新打開連線
                if (command.Connection.State == ConnectionState.Closed)
                {
                    Console.WriteLine(">>> Database重新連線...");
                    command.Connection.Open();
                }
            }
        }
    }
}
