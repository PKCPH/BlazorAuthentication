using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Security.Cryptography;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorAuthenticationWebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EncryptController : ControllerBase
{


    // POST api/<EncryptController>
    [HttpPost]
    public Task<string[]> Post([FromBody] string[] value)
    {
        string txtToEncrypt = value[0];
        string publicKey = value[1];
        
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(publicKey);

        byte[] txtToEncryptAsByteArray = System.Text.Encoding.UTF8.GetBytes(txtToEncrypt);
        byte[] encryptedValue = rsa.Encrypt(txtToEncryptAsByteArray, true);
        string encryptedValueAsString = Convert.ToBase64String(encryptedValue);

        return encryptedValueAsString;
    }

}
