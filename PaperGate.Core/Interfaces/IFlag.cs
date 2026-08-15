namespace PaperGate.Core.Interfaces
{
    public interface ISoftDeleteDatabaseModel
    {
        public bool IsDeleted { get; set; }
    }
    public interface IBaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
