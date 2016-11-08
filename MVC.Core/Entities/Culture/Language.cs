namespace MVC.Core.Entities.Culture
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Language : BaseEntity
	{
        [Required, MaxLength(5), RegularExpression("^[a-z]{2}-[a-z]{2}$", ErrorMessage = "Must be a valid language code - e.g. en-gb, fr-fr.")]
        [Index(IsUnique = true), Column(TypeName = "char")]
        public string LanguageCode { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string RegionName { get; set; }

        [Required, MaxLength(50)]
        public string LocalName { get; set; }

        [MaxLength(50)]
        public string LocalRegionName { get; set; }

        public bool IsSupported { get; set; }
    }
}
