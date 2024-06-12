using Frontend.Web.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Frontend.Web.Models.Functions.CommonEnumsUI;
using static System.Net.Mime.MediaTypeNames;

namespace Frontend.Web.Models.Functions
{
  public static class CommonFunctionUI
  {
    public static string DashboardUrl(string roleName)
    {
      string url = "";

     
        url = "~/Dashboard/Index";
      
      return url;
    }
    public static string DashboardName(string roleName)
    {
      string url = "";


      
        url = "Dashboard";
      

      return url;
    }
    public static DateTime GetPortugalDateTime()
    {
      DateTime date;
      date = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Azores Standard Time"));
      return date;
    }
    public static List<string> ErrorResponseToErrorResult(ResponseMessage errorResponse)
    {
      List<string> errors = new List<string>();
      if (errorResponse.StatusCode == 400 && errorResponse.StatusTitle == "BadRequest")
      {
        errors.Add(errorResponse.StatusMessage);
      }
      else if (errorResponse.StatusCode == 400)
      {
        errors.Add("username and password is missing");
      }
      else if (errorResponse.StatusCode == 401)
      {
        errors.Add("invalid username or password");
      }
      else if (errorResponse.StatusCode == 402)
      {
        errors.Add("please fill out the form");
      }
      else if (errorResponse.StatusCode == 403)
      {
        errors.Add(errorResponse.StatusMessage);
      }
      return errors;
    }
    public static ResponseMessage JsonResponse(ResponseType responseType, string message, string guid)
    {
      ResponseMessage responseMessage = new ResponseMessage();
      if (responseType == ResponseType.Success)
      {
        responseMessage.StatusCode = 200;
        responseMessage.StatusTitle = "success";
        responseMessage.StatusMessage = message;
        responseMessage.Message = guid;
      }
      if (responseType == ResponseType.Failure)
      {
        responseMessage.StatusCode = 400;
        responseMessage.StatusTitle = "BadRequest";
        responseMessage.StatusMessage = message;
      }
      if (responseType == ResponseType.SuccessId)
      {
        responseMessage.StatusCode = 200;
        responseMessage.StatusTitle = "success";
        responseMessage.StatusMessage = message;
        responseMessage.Id = Convert.ToInt64(guid);

      }
      if (responseType == ResponseType.SuccessCreated)
      {
        responseMessage.StatusCode = 201;
        responseMessage.StatusTitle = "success";
        responseMessage.StatusMessage = message;
        responseMessage.Id = Convert.ToInt64(guid); 

      }
      return responseMessage;
    }
    public static string JsonResponse(string message)
    {
      string responseMessage = "";
      string[] _split = message.Split(',');
      responseMessage = _split[1].ToString();
      return responseMessage;
    }
    public static ResponseMessage GetIdfromJsonResponse(ResponseType responseType, string message)
    {
      ResponseMessage responseMessage = new ResponseMessage();
      if (responseType == ResponseType.Success)
      {
        responseMessage.StatusCode = 200;
        responseMessage.StatusTitle = "success";
        responseMessage.StatusMessage = message;
      }
      if (responseType == ResponseType.Failure)
      {
        responseMessage.StatusCode = 400;
        responseMessage.StatusTitle = "BadRequest";
        responseMessage.StatusMessage = message;
      }
      return responseMessage;
    }
    public static Guid GetIdFromClaim(string value)
    {
      Guid Id = Guid.Empty;
      if (!String.IsNullOrEmpty(value))
      {
        Id = Guid.Parse(value);
      }

      return Id;
    }
    public static bool IsValidGuid(object value)
    {
      if (value is null)
        return false; // Allows to return a null value
      switch (value)
      {
        case Guid guid:
          return guid != Guid.Empty; //Checks whether the GUID is empty or not and returns false if GUID is empty
        default:
          return true;
      }
    }
    public static bool IsValidLong(object value)
    {
      if (value is null)
        return false; // Allows to return a null value
      switch (value)
      {
        case long id:
          return id != 0; //Checks whether the GUID is empty or not and returns false if GUID is empty
        default:
          return true;
      }
    }
    public static bool DeleteFile(string basePath, string imagePath)
    {
      bool isDone = false;
      if (!String.IsNullOrEmpty(imagePath) && File.Exists(basePath + imagePath))
      {
        File.Delete(basePath + imagePath);
        isDone = true;
      }
      return isDone;
    }
    public static void DeleteImage(string basePath, string imagePath)
    {

      if (!String.IsNullOrEmpty(imagePath) && File.Exists(basePath + imagePath))
      {
        File.Delete(basePath + imagePath);
      }
    }
    public static string GetNameOfEnumByValue<T>(int enumValue)
    {
      return Enum.GetName(typeof(T), enumValue);
    }
    public static TAttribute GetEnumDispalyAttribute<TAttribute>(Enum enumValue) where TAttribute : Attribute
    {
      return enumValue.GetType()
                      .GetMember(enumValue.ToString())
                      .First()
                      .GetCustomAttribute<TAttribute>();
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
    public static string ToDisplayName(this Enum value)
    {
      var attribute = value.GetAttribute<DisplayAttribute>();
      return attribute == null ? value.ToString() : attribute.Name;
    }

    public static string ConvertDateFormate(string value)
    {
      string date = "";
      if (!String.IsNullOrEmpty(value))
      {
        date = Convert.ToDateTime(value).ToString("dd-MM-yyyy");
      }
      return date;
    }
    public static void Log(string _logMessage)
    {
      File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "Log_" + DateTime.Now.ToString("MM-dd-yyyy") + ".txt", string.Format("{0}{1}", DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss") + "==>" + _logMessage, Environment.NewLine));

    }
    
    public static string ConvertDateTimeUItoAPI(string _date)
    {
      string _dateFormat = "dd-MM-yyyy";
      DateTime date;
      date = DateTime.ParseExact(_date, _dateFormat, CultureInfo.InvariantCulture);
      return date.ToString();
    }


    //public static Stream ToStream(this Image image)
    //{
    //  try
    //  {
    //    var stream = new MemoryStream();

    //    image.Save(stream, ImageFormat.Png);
    //    stream.Position = 0;

    //    return stream;
    //  }
    //  catch (Exception ex)
    //  {

    //    throw;
    //  }

    //}
    public static string TimeAgo(DateTime dateTime)
    {
      string result = string.Empty;
      var timeSpan = GetPortugalDateTime().Subtract(dateTime);

      if (timeSpan <= TimeSpan.FromSeconds(60))
      {
        result = string.Format("{0} seconds ago", timeSpan.Seconds);
      }
      else if (timeSpan <= TimeSpan.FromMinutes(60))
      {
        result = timeSpan.Minutes > 1 ?
            String.Format("about {0} minutes ago", timeSpan.Minutes) :
            "about a minute ago";
      }
      else if (timeSpan <= TimeSpan.FromHours(24))
      {
        result = timeSpan.Hours > 1 ?
            String.Format("about {0} hours ago", timeSpan.Hours) :
            "about an hour ago";
      }
      else if (timeSpan <= TimeSpan.FromDays(30))
      {
        result = timeSpan.Days > 1 ?
            String.Format("about {0} days ago", timeSpan.Days) :
            "yesterday";
      }
      else if (timeSpan <= TimeSpan.FromDays(365))
      {
        result = timeSpan.Days > 30 ?
            String.Format("about {0} months ago", timeSpan.Days / 30) :
            "about a month ago";
      }
      else
      {
        result = timeSpan.Days > 365 ?
            String.Format("about {0} years ago", timeSpan.Days / 365) :
            "about a year ago";
      }

      return result;
    }
    #region Region_Cliams
    public static ClaimsInfoModel LoginUsersClaims(List<Claim> listClaims)
    {
      ClaimsInfoModel model = new ClaimsInfoModel();
      if (listClaims.Count > 0)
      {
        model.UserName = listClaims.Where(c => c.Type == LoginUserClaim.UserName).Select(x => x.Value).FirstOrDefault();
        model.Password = listClaims.Where(c => c.Type == LoginUserClaim.Password).Select(x => x.Value).FirstOrDefault();
        model.AccessToken = listClaims.Where(c => c.Type == LoginUserClaim.AccessToken).Select(x => x.Value).FirstOrDefault();
        model.RefreshToken = listClaims.Where(c => c.Type == LoginUserClaim.RefreshToken).Select(x => x.Value).FirstOrDefault();
        model.DisplayName = listClaims.Where(c => c.Type == ClaimTypes.Name).Select(x => x.Value).FirstOrDefault();
        model.DasboardName = listClaims.Where(c => c.Type == LoginUserClaim.DasboardName).Select(x => x.Value).FirstOrDefault();
        model.UserId = Convert.ToInt64(listClaims.Where(c => c.Type == LoginUserClaim.UserId).Select(x => x.Value).FirstOrDefault());
        model.UserProfile = listClaims.Where(c => c.Type == LoginUserClaim.UserProfile).Select(x => x.Value).FirstOrDefault();

      }
      return model;
    }
    
    public static ClaimsPrincipal ClaimsUpdate_NameType(ClaimsIdentity identity, string keyValue)
    {
      Claim claim = identity.FindFirst(ClaimTypes.Name);
      identity.RemoveClaim(claim);
      identity.AddClaim(new Claim(ClaimTypes.Name, keyValue));

      var userPrincipal = new ClaimsPrincipal(new[] { identity });

      return userPrincipal;
    }
    public static ClaimsPrincipal ClaimsUpdate_Type(ClaimsIdentity identity, string key, string keyValue)
    {
      Claim claim = identity.FindFirst(key);
      if (claim != null)
      {
        identity.RemoveClaim(claim);
      }

      identity.AddClaim(new Claim(key, keyValue));

      var userPrincipal = new ClaimsPrincipal(new[] { identity });

      return userPrincipal;
    }
    #endregion
    public static string ExceptionMessage(Exception ex)
    {
      string violationMessage = String.Empty;
      var message = ex.Message;
      var innerException = ex.InnerException;
      while (innerException != null)
      {
        message = innerException.Message;
        innerException = innerException.InnerException;
      }
      bool PrimaryKey = message.Contains("Violation of PRIMARY KEY");
      bool ForginKey = message.Contains("REFERENCE");
      bool UniqueKey = message.Contains("UNIQUE KEY");
      if (PrimaryKey || UniqueKey)
      {
        violationMessage = "This Record is already added in Database.";
      }
      else
      {
        string[] arr = message.Split('.');
        if (arr.Length > 0)
        {
          violationMessage = arr[0];
        }
      }
      return violationMessage;
    }
  }
}
