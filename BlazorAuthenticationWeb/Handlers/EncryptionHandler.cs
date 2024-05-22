using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace BlazorAuthenticationWeb.Handlers;

public class EncryptionHandler
{
    private readonly IDataProtector _dataProtector;
    private string _privateKey;
    private string _publicKey;
    private readonly HttpClient _httpClient;
    public EncryptionHandler(IDataProtectionProvider dataProtector)
    {
        _dataProtector = dataProtector.CreateProtector("LOLKey");

        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        _privateKey = rsa.ToXmlString(true);
        _publicKey = rsa.ToXmlString(false);
    }

    #region Symmetric encryption
    
    public string SymmetricEncryption(string txtToEncrypt)
    {
        return _dataProtector.Protect(txtToEncrypt);
    }
    public string SymmetricDecryption(string txtToEncrypt)
    {
        return _dataProtector.Unprotect(txtToEncrypt);
    }

    #endregion

    #region Asymmetric encryption

    public async Task<string> EncryptAsymetriscParent(string txtToEncrypt)
    {
        string[] data = new string[2] { txtToEncrypt, _publicKey };
        string serializedValue = JsonConvert.SerializeObject(data);
        StringContent content = new StringContent(serializedValue, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://localhost:7171/api/Encrypt", content);
        string encryptedValue = await response.Content.ReadAsStringAsync();
        return encryptedValue;
    }

    public string EncryptAsymetrisc(string txtToDecrypt)
    {

        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(_privateKey);

        byte[] txtToDecryptAsByteArry = System.Text.Encoding.UTF8.GetBytes(txtToDecrypt);
        byte[] decryptedValue = rsa.Decrypt(txtToDecryptAsByteArry, true);
        string decryptedValueAsString = Convert.ToBase64String(decryptedValue);

        return decryptedValueAsString;
    }
    #endregion

}
