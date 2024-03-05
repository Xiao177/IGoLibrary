using RestSharp;
using IGoLibrary_Winform.CustomException;

namespace IGoLibrary_Winform.Controller
{
    public class GetCookieServiceImpl : IGetCookieService
    {
        public string GetCookie(string code)
        {
            var client = new RestClient(string.Format("https://libseat.shnu.edu.cn/index.php/urlNew%2Fauth.html%3Fr%3Dhttps%253A%252F%252Flibseat.shnu.edu.cn%252Fweb%2Findex.html&code={0}&state=1",code));
            var request = new RestRequest();
            request.Method = Method.Get;
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(5000);//设定超时时间为5000ms
            RestResponse response = client.Execute(request,cts.Token);
            var cookieCollection = response.Cookies;
            if(cookieCollection != null)
            {
                if(cookieCollection.Count >= 2)
                {
                    return cookieCollection[1].ToString() + "; " + cookieCollection[0].ToString();
                }
                else
                    throw new GetCookieException("Cookie不包含关键身份信息，可能是code过期，重新填写含code的链接");
            }
            else
                throw new GetCookieException("响应报文返回的Cookie为空");
        }
    }
}
