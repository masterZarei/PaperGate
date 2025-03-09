using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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
