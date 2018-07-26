using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    public class CreditCard
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Card Number")]
        [RegularExpression(@"^(\d{13,16})$", ErrorMessage = "Card Number must be of length 13 to 16")]
        public string CreditCardId { get; set; }

        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:Y}", ApplyFormatInEditMode = true)]
        public DateTime ExpiryDate { get; set; }
        [RegularExpression(@"^(\d{3})$", ErrorMessage = "CVV must be of length 3")]
        public int CVV { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public virtual Lodger Lodger { get; set; }
       
    }
}
