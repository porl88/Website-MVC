namespace MVC.Core.Entities.Location
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using Culture;

    public class Country : BaseEntity<string>
    {
        [Column("IsoCode")]
        [MaxLength(2), RegularExpression("^[A-Z]{2}$")]
        public override string Id { get; set; }

        [Index(IsUnique = true), Column(TypeName = "char")]
        [MaxLength(3), Required, RegularExpression("^[A-Z]{3}$")]
        public string IsoCode3A { get; set; }

        [Index(IsUnique = true), Column(TypeName = "char")]
        [MaxLength(3), Required, RegularExpression("^[0-9]{3}$")]
        public string IsoCode3N { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(10)]
        public string DiallingCode { get; set; }

        public Currency Currency { get; set; }
    }
}
