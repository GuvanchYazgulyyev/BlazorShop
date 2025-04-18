using System;

namespace ShopSharedLibrary.DataObject.ResponseModels
{
    public class ApiExeption : Exception
    {
        public ApiExeption(string message, Exception exception) : base(message, exception)
        {

        }
        public ApiExeption(string message) : base(message)
        {

        }
    }
}
