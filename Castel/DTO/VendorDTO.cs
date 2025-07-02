namespace Castel.DTO
{
    public class VendorDTO
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
    public class AddVendorDTO : VendorDTO { }
    public class UpdateVendorDTO : VendorDTO { }
    public class GetVendorDTO : VendorDTO
    {
        public long id;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
