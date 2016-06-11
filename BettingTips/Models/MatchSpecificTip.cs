using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BettingTips.Models
{
    public class MatchSpecificTip : IValidatableObject
    {
        public int Id { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(900), Required]
        public string Tip { get; set; }

        [Index, Required]
        public DateTime SendTime { get; set; }

        [Index, Required]
        public DateTime Expiration { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (var db = new ApplicationDbContext())
            {
                // SendTime should be some time in the future
                if (SendTime < DateTime.Now)
                {
                    yield return new
                        ValidationResult("The Send time should be sometime in the future.");
                }

                // Expiration date should be greater than SendTime
                TimeSpan interval = Expiration - SendTime;
                if (interval.Hours < 2)
                {
                    yield return new
                        ValidationResult("The expiration should be at least two hours after the send time");
                }

                // Ensure no other entry in the database contains
                if (db.MatchSpecificTips.Max(mst => mst.SendTime) > SendTime)
                {
                    yield return new ValidationResult("New match tips cannot have send times" + 
                        " earlier than existing match tips!");
                }
            }
        }
    }
}