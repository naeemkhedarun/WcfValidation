using System;
using System.Collections.Generic;

namespace WcfValidation.Validation.Attributes
{
    public class ProductItemValidatorAttribute : RestArgumentValidatorBase
    {
        public override IEnumerable<Type> Validators
        {
            get { return new[] {typeof (ProductItemValidator)}; }
        }
    }
}