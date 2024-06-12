using Backend.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Backend.Common.CommonFunctions.CommonEnums;

namespace Backend.Common.CommonFunctions
{
  public  static class CommonFunction
  {
    public static ResponseMessage Response(ResponseType responseType, string message)
    {
      ResponseMessage responseMessage = new ResponseMessage();
      if (responseType == ResponseType.Success)
      {
        responseMessage.StatusCode = 200;
        responseMessage.StatusTitle = "success";
        responseMessage.StatusMessage = "successfully completed, " + message;
        responseMessage.Message = message;
      }
      if (responseType == ResponseType.Failure)
      {
        responseMessage.StatusCode = 400;
        responseMessage.StatusTitle = "BadRequest";
        responseMessage.StatusMessage = message;
        responseMessage.Message = message;
      }
      if (responseType == ResponseType.SuccessId)
      {
        responseMessage.StatusCode = 200;
        responseMessage.StatusTitle = "success";
        responseMessage.Message = message;
        responseMessage.StatusMessage = "successfully completed, " + message;
        responseMessage.Id = Convert.ToInt64(message);
      }
      if (responseType == ResponseType.FailureId)
      {
        responseMessage.StatusCode = 400;
        responseMessage.StatusTitle = "BadRequest";
        responseMessage.Message = message;
        var readmessage = message.Split(",");

        responseMessage.StatusMessage = readmessage[0];
        responseMessage.Id = Convert.ToInt64(readmessage[1]);
        responseMessage.Message = readmessage[2];
      }
      return responseMessage;
    }
    public static ResponseMessage Response_Custom(ResponseType responseType, long id, string message)
    {
      ResponseMessage responseMessage = new ResponseMessage();
      if (responseType == ResponseType.Success)
      {
        responseMessage.StatusCode = 200;
        responseMessage.StatusTitle = "success";
        responseMessage.StatusMessage = "successfully completed";
        responseMessage.Message = message;
        responseMessage.Id = id;
      }
      if (responseType == ResponseType.SuccessCreated)
      {
        responseMessage.StatusCode = 201;
        responseMessage.StatusTitle = "success";
        responseMessage.StatusMessage = "successfully completed";
        responseMessage.Message = message;
        responseMessage.Id = id;
      }
      if (responseType == ResponseType.SuccessId)
      {
        responseMessage.StatusCode = 200;
        responseMessage.StatusTitle = "success";
        responseMessage.StatusMessage = "successfully completed";
        responseMessage.Message = message;
        responseMessage.Id = id;
      }
      if (responseType == ResponseType.Failure)
      {
        responseMessage.StatusCode = 400;
        responseMessage.StatusTitle = "BadRequest";
        responseMessage.StatusMessage = message;
        responseMessage.Message = message;
        responseMessage.Id = id;
      }
      return responseMessage;
    }



    public static string GenerateRandomNo()
    {
      Random random = new Random((int)DateTime.Now.Ticks);

      //Generate your random number
      int code = random.Next(0, 99999);

      //Output the random number including leading zeroes (it should be a string if you want the leadings zeros)
      return code.ToString("D6");
    }
    public static int GetValueOfEnumByName<T>(string enumName)
    {
      return (int)Enum.Parse(typeof(T), enumName, true);
    }
    public static T GetAttribute<T>(this Enum value) where T : Attribute
    {
      var type = value.GetType();
      var memberInfo = type.GetMember(value.ToString());
      var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
      return (T)attributes.FirstOrDefault();//attributes.Length > 0 ? (T)attributes[0] : null;
    }
    public static string GetNameOfEnumByValue<T>(int enumValue)
    {
      return Enum.GetName(typeof(T), enumValue);
    }

  }
}
