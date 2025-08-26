using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using NLog;

namespace Quantumart.QP8.DAL;

public class DbLoggingInterceptor : DbCommandInterceptor
{
    private const string Insert = "insert";
    private const string Select = "select";
    private const string Update = "update";
    private const string Delete = "delete";
    private const string Migration = "migration";

    private const string DatabaseProperty = "database";
    private const string MethodProperty = "method";
    private const string CommandTextProperty = "commandText";
    private const string ParametersProperty = "parameters";
    private const string DurationProperty = "duration";
    private const string ExceptionProperty = "exception";
    private const string EventIdProperty = "eventId";

    public const int DatabaseOperationStart = 700;
    public const int DatabaseOperationSuccessful = 701;
    public const int DatabaseOperationFailed = 702;

    public static readonly EventId DatabaseOperationStartEventId = new EventId(DatabaseOperationStart, nameof(DatabaseOperationStart));
    public static readonly EventId DatabaseOperationSuccessfulEventId = new EventId(DatabaseOperationSuccessful, nameof(DatabaseOperationSuccessful));
    public static readonly EventId DatabaseOperationFailedEventId = new EventId(DatabaseOperationFailed, nameof(DatabaseOperationFailed));

    private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();

    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        LogDatabaseStart(command, eventData);
        return base.ReaderExecuting(command, eventData, result);
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        LogDatabaseStart(command, eventData);
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        LogDatabaseSuccessful(command, eventData);
        return base.ReaderExecuted(command, eventData, result);
    }

    public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = new CancellationToken())
    {
        LogDatabaseSuccessful(command, eventData);
        return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }

    public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
    {
        LogDatabaseError(command, eventData);
        base.CommandFailed(command, eventData);
    }

    public override Task CommandFailedAsync(DbCommand command, CommandErrorEventData eventData, CancellationToken cancellationToken = new CancellationToken())
    {
        LogDatabaseError(command, eventData);
        return base.CommandFailedAsync(command, eventData, cancellationToken);
    }

    private void LogDatabaseStart(DbCommand command, CommandEventData eventData)
    {
        var systemMethod = GetSystemMethod(command, eventData);

        var parameters = new StringBuilder();

        for (int i = 0; i < command.Parameters.Count; i++)
        {
            var param = command.Parameters[i];
            parameters
                .Append(param.ParameterName)
                .Append('=')
                .Append(param.Value)
                .Append(", ");
        }

        Logger.ForInfoEvent()
           .Message("Database operation started")
           .Property(EventIdProperty, DatabaseOperationStartEventId)
           .Property(DatabaseProperty, command.Connection.Database)
           .Property(MethodProperty, systemMethod)
           .Property(CommandTextProperty, command.CommandText)
           .Property(ParametersProperty, parameters.ToString())
           .Log();
    }

    private void LogDatabaseSuccessful(DbCommand command, CommandExecutedEventData eventData)
    {
        var systemMethod = GetSystemMethod(command, eventData);

        Logger.ForInfoEvent()
           .Message("Database operation completed successfully")
           .Property(EventIdProperty, DatabaseOperationSuccessfulEventId)
           .Property(DatabaseProperty, command.Connection.Database)
           .Property(MethodProperty, systemMethod)
           .Property(DurationProperty, eventData.Duration)
           .Log();
    }

    private void LogDatabaseError(DbCommand command, CommandErrorEventData eventData)
    {
        var systemMethod = GetSystemMethod(command, eventData);

        Logger.ForInfoEvent()
           .Message("Database operation completed with error")
           .Property(EventIdProperty, DatabaseOperationFailedEventId)
           .Property(DatabaseProperty, command.Connection.Database)
           .Property(MethodProperty, systemMethod)
           .Property(DurationProperty, eventData.Duration)
           .Property(ExceptionProperty, eventData.Exception)
           .Log();
    }

    private static string GetSystemMethod(DbCommand command, CommandEventData eventData)
    {
        var method = string.Empty;
        if (eventData.CommandSource == CommandSource.Migrations)
        {
            method = Migration;
        }
        else if (command.CommandText.StartsWith("UPDATE"))
        {
            method = Update;
        }
        else if (command.CommandText.StartsWith("DELETE"))
        {
            method = Delete;
        }
        else if (command.CommandText.StartsWith("SELECT"))
        {
            method = Select;
        }
        else if (command.CommandText.StartsWith("INSERT"))
        {
            method = Insert;
        }

        return method;
    }
}
