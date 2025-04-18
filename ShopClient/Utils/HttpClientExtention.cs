using ShopSharedLibrary.DataObject.ResponseModels;
using System.Net.Http.Json;
using System.Net.Http;
namespace ShopClient.Utils
{
    public static class HttpClientExtention
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="url"></param>
        /// <param name="throwValue"></param>
        /// <returns></returns>
        /// <exception cref="ApiExeption"></exception>
        public async static Task<T> GetServiceResponseAsync<T>(this HttpClient httpClient, String url, bool throwValue = false)
        {
            var http = await httpClient.GetFromJsonAsync<ServiceResponse<T>>(url);
            if (!http.IsSuccess && throwValue)
                //throw new Exception(http.Message);
                throw new ApiExeption(http.Message);
            return http.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="url"></param>
        /// <param name="value"></param>
        /// <param name="throwExep"></param>
        /// <returns></returns>
        /// <exception cref="ApiExeption"></exception>
        public async static Task<BaseResponse> PostServiceResponseAsync<TResult, TValue>(this HttpClient httpClient, string url, TValue value, bool throwExep = false)
        {
            var httpR = await httpClient.PostAsJsonAsync(url, value);
            if (httpR.IsSuccessStatusCode)
            {
                var res = await httpR.Content.ReadFromJsonAsync<ServiceResponse<TResult>>();
                if (!!res.IsSuccess)
                    return !res.IsSuccess && throwExep ? throw new ApiExeption(res.Message) : res; // Value
            }
            throw new ApiExeption(httpR.StatusCode.ToString());
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="url"></param>
        /// <param name="value"></param>
        /// <param name="throwExep"></param>
        /// <returns></returns>
        /// <exception cref="ApiExeption"></exception>

        public async static Task<BaseResponse> PostBaseResponseAsync<TValue>(this HttpClient httpClient, string url, TValue value, bool throwExep = false)
        {
            var httpR = await httpClient.PostAsJsonAsync(url, value);
            if (httpR.IsSuccessStatusCode)
            {
                var res = await httpR.Content.ReadFromJsonAsync<BaseResponse>();
                return !res.IsSuccess && throwExep ? throw new ApiExeption(res.Message) : res;
            }
            throw new ApiExeption(httpR.StatusCode.ToString());
        }
    }
}
