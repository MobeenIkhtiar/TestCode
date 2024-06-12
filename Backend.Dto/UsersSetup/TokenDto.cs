using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Dto.UsersSetup
{
  public class TokenDto
  {
    [JsonPropertyName("refreshToken")]
    [Required]
    public string RefreshToken { get; set; }
  }
}
