using Backend.Services.JwtServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Manager.JwtManager
{
  public class SecurityManager : ISecurityService
  {
    private readonly RandomNumberGenerator _rand = RandomNumberGenerator.Create();

    public string GetSha256Hash(string input)
    {
      using var hashAlgorithm = new SHA256CryptoServiceProvider();
      var byteValue = Encoding.UTF8.GetBytes(input);
      var byteHash = hashAlgorithm.ComputeHash(byteValue);
      return Convert.ToBase64String(byteHash);
    }

    public Guid CreateCryptographicallySecureGuid()
    {
      var bytes = new byte[16];
      _rand.GetBytes(bytes);
      return new Guid(bytes);
    }
  }
}
