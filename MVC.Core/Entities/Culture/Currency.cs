namespace MVC.Core.Entities.Culture
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Currency : BaseEntity<string>
    {
        [Column("IsoCode")]
        [MaxLength(2), RegularExpression("^[A-Z]{3}$")]
        public override string Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(1)]
        public string Symbol { get; set; }
    }
}
