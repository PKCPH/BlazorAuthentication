using BCrypt.Net;
using System.Security.Cryptography;
using System.Text;

namespace BlazorAuthenticationWeb.Handlers;

public class HashingHandler
{
    public string MD5Hashing(string txtToHash)
    {
        MD5 md5 = MD5.Create();
        byte[] txtToHashAsByteArray = Encoding.ASCII.GetBytes(txtToHash);
        byte[] hashedValue = md5.ComputeHash(txtToHashAsByteArray);
        return Convert.ToBase64String(hashedValue);

    }

    public string SHA2Hashing(string txtToHash)
    {
        SHA256 sha256 = SHA256.Create();
        byte[] txtToHashAsByteArray = Encoding.ASCII.GetBytes(txtToHash);
        byte[] hashedValue = sha256.ComputeHash(txtToHashAsByteArray);
        return Convert.ToBase64String(hashedValue);
    }

    //har kun en key, immitere symmetrisk kryptering
    public string HMACHashing(string txtToHash)
    {
        byte[] myKey = Encoding.ASCII.GetBytes("LOL");
        byte[] txtToHashAsByteArray = Encoding.ASCII.GetBytes(txtToHash);

        HMACSHA256 hmac = new HMACSHA256();
        hmac.Key = myKey;
        byte[] hashedValue = hmac.ComputeHash(txtToHashAsByteArray);
        return Convert.ToBase64String(hashedValue);
    }

    //udvid db med salt property
    public string PBKDF2Hashing(string txtToHash)
    {
        byte[] salt = Encoding.ASCII.GetBytes("LOL");
        byte[] txtToHashAsByteArray = Encoding.ASCII.GetBytes(txtToHash);
        var hashAlgo = new HashAlgorithmName("SHA256");
        int itirations = 10;
        int outputLength = 32;

        byte[] hashedValue = Rfc2898DeriveBytes.Pbkdf2(txtToHashAsByteArray, salt, itirations, hashAlgo, outputLength);
        return Convert.ToBase64String(hashedValue);

    }

    public string BCryptHashing(string txtToHash)
    {
        ////First example
        //return BCrypt.Net.BCrypt.HashPassword(txtToHash);

        ////Second example
        //int workFactor = 10;
        //bool enhancedEntropy = true;
        //return BCrypt.Net.BCrypt.HashPassword(txtToHash, workFactor, enhancedEntropy);

        //Third example
        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        bool enhancedEntropy = true;
        HashType hashType = HashType.SHA256;

        return BCrypt.Net.BCrypt.HashPassword(txtToHash, salt, enhancedEntropy, hashType);

    }

    public bool BCryptHashingVerify(string txtToHash, string hashedValueAsString)
    {
        ////First example
        //return BCrypt.Net.BCrypt.Verify(txtToHash, hashedValueAsString);

        ////Second example
        //return BCrypt.Net.BCrypt.Verify(txtToHash, hashedValueAsString, true);

        //Third example
        return BCrypt.Net.BCrypt.Verify(txtToHash, hashedValueAsString, true, HashType.SHA256);
    }
}
;