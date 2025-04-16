using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSharedLibrary.DataObject.ResponseModels
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            IsSuccess = true;
        }
        public bool IsSuccess { get; set; }
        public String Message { get; set; }

        public void SetExeption(Exception exeption)
        {
            IsSuccess = false;
            Message = exeption.Message;
        }
    }
}
