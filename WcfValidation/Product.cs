using System.ServiceModel.Activation;
using WcfValidation.Validation.Attributes;
using WcfValidation.Validation.Framework;

namespace WcfValidation
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Product : IProduct
    {
        [RestArgumentValidation]
        public ProductItem Create([ProductItemValidator] ProductItem instance)
        {
            return new ProductItem
                       {
                           Name = instance.Name
                       };
        }
    }
}
