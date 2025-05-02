using PaperGate.Core.Entities;
using System.Linq.Expressions;

namespace PaperGate.Core.Interfaces.Repositories;
public interface IMyLoggerRepository
{
    Task Log(string section, string message, LoggingLevel severity);
    Task<List<MyLoggerInfo>> GetLogs(Expression<Func<MyLoggerInfo, bool>>? filter = null);
    Task<MyLoggerInfo> Get(int Id);
}