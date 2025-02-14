using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTrackingSystem.Shared.Common;

public class ResponseModel
{
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
}

public class SubResponseModel
{
    public static ResponseModel GetResponseMsg(string message, bool isSuccess)
    {
        return new ResponseModel()
        {
            Message = message,
            IsSuccess = isSuccess
        };
    }
}
