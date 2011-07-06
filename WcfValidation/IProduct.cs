using System.ServiceModel;
using System.ServiceModel.Web;

namespace WcfValidation
{
    [ServiceContract]
    public interface IProduct
    {
        [WebInvoke(UriTemplate = "/product/", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        ProductItem Create(ProductItem instance);
    }
}