using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace PaperGate.Core.Interfaces
{
    public interface ISoftDeleteDatabaseModel
    {
        public bool IsDeleted { get; set; }
    }
    public interface IDatabaseModel<T> where T : class
    {
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
    public interface IMultipleUploadDto
    {
        public List<IFormFile>? MultipleFilesUp { get; set; }
    }
    public interface IBaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
