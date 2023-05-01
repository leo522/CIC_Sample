namespace CIC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CICBOOK")]
    public partial class CICBOOK
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EID { get; set; }

        [StringLength(10)]
        public string NAME_ZH { get; set; }

        [StringLength(50)]
        public string RECEIPT_LIST { get; set; }

        [StringLength(20)]
        public string NO_LIST { get; set; }

        public DateTime? EFFECTIVE_STARTDATE { get; set; }

        public DateTime? EFFECTIVE_ENDDATE { get; set; }

        [StringLength(50)]
        public string DEPT { get; set; }
    }
}
