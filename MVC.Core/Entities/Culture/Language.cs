namespace MVC.Core.Entities.Culture
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Language : BaseEntity<string>
    {
        [Column("IsoCode")]
        [MaxLength(3), RegularExpression("^[a-z]{2,3}$", ErrorMessage = "Must be a valid 2-letter language code - e.g. en, fr, de.")]
        public override string Id { get; set; }

        [Column(TypeName = "char")]
        [MaxLength(3), RegularExpression("^[a-z]{3}$", ErrorMessage = "Must be a valid 3-letter language code - e.g. eng, fra, deu.")]
        public string IsoCode3 { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string NativeName { get; set; }
    }
}
