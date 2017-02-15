namespace MVC.Core.Entities.Culture
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Language : BaseEntity<string>
    {
        [Column("IsoCode")]
        [MaxLength(2), RegularExpression("^[a-z]{2}$", ErrorMessage = "Must be a valid language code - e.g. en, fr, de.")]
        public override string Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string LocalName { get; set; }
    }
}
