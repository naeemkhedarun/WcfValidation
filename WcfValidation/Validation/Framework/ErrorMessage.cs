using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WcfValidation.Validation.Framework
{
    [DataContract]
    public class ErrorMessage
    {
        public ErrorMessage()
        {
            Errors = new List<string>();
        }

        [DataMember]
        public string ParameterName { get; set; }
        
        [DataMember]
        public ICollection<string> Errors { get; set; }
    }
}