using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    public class CustomValidation
    {
        public List<ValidationResult> Validate()
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            Validator.TryValidateObject(this, new ValidationContext(this, null, null), errors, true);

            return errors;
        }

        public bool IsValid()
        {
            return Validator.TryValidateObject(this, new ValidationContext(this, null, null), null, true);
        }
    }
}