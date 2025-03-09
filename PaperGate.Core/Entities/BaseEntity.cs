using PaperGate.Core.Interfaces;

namespace PaperGate.Core.Entities;
public abstract class BaseEntity : IBaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
}
