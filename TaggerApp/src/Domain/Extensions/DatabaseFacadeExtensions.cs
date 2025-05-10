using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Domain.Extensions;

public static class DatabaseFacadeExtensions {
    public static string GetDatabasePath(this DatabaseFacade database) {
        var connectionString = database.GetConnectionString();
        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("Database connection string is null or empty.");

        return connectionString.Substring(connectionString.IndexOf('=') + 1);
    }
}