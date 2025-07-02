namespace Castel.DTO
{
    public class DivisionDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
    public class AddDivisionDTO : DivisionDTO { }
    public class UpdateDivisionDTO : DivisionDTO { }
    public class GetDivisionDTO : DivisionDTO
    {
        public long id;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
