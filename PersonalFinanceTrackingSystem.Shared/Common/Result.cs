namespace PersonalFinanceTrackingSystem.Shared.Common;

public class Result<T>
{
   public bool Success { get; set; }
   public string Message { get; set; }
   public T Data { get; set; }

   public static Result<T> SuccessResult(string message = "Operation successful")
   {
      return new Result<T> { Success = true, Message = message };
   }

   public static Result<T> SuccessResult(T data, string message = "Operation successful")
   {
      return new Result<T> { Success = true, Message = message, Data = data };
   }

   public static Result<T> FailureResult(string message)
   {
      return new Result<T> { Success = false, Message = message };
   }
   
}