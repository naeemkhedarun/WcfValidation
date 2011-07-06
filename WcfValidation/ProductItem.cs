using System.Runtime.Serialization;

namespace WcfValidation
{
    [DataContract]
    public class ProductItem
    {
        [DataMember]
        public string Name { get; set; }
    }
}