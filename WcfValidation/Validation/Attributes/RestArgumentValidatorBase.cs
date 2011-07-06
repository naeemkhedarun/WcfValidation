using System;
using System.Collections.Generic;

namespace WcfValidation.Validation.Attributes
{
    [Serializable]
    public abstract class RestArgumentValidatorBase : Attribute
    {
        public abstract IEnumerable<Type> Validators { get; }

        public virtual Func<object> DefaultValue { get; private set; }
    }
}