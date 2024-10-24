﻿using System.Security.Cryptography;
using System.Text;

namespace EShop.Application.Common.Helpers;

public static class PasswordHelper
{
    public static byte[] Pbkdf2Sha256GetBytes(byte[] password, byte[] salt, int iterationCount, int dklen)
    {
        using (var hmac = new HMACSHA256(password))
        {
            int hashLength = hmac.HashSize / 8;
            if ((hmac.HashSize & 7) != 0)
                hashLength++;
            int keyLength = dklen / hashLength;
            if (dklen > 0xFFFFFFFFL * hashLength || dklen < 0)
                throw new ArgumentOutOfRangeException("dklen");
            if (dklen % hashLength != 0)
                keyLength++;
            byte[] extendedkey = new byte[salt.Length + 4];
            Buffer.BlockCopy(salt, 0, extendedkey, 0, salt.Length);
            using (var ms = new MemoryStream())
            {
                for (int i = 0; i < keyLength; i++)
                {
                    extendedkey[salt.Length] = (byte)(i + 1 >> 24 & 0xFF);
                    extendedkey[salt.Length + 1] = (byte)(i + 1 >> 16 & 0xFF);
                    extendedkey[salt.Length + 2] = (byte)(i + 1 >> 8 & 0xFF);
                    extendedkey[salt.Length + 3] = (byte)(i + 1 & 0xFF);
                    byte[] u = hmac.ComputeHash(extendedkey);
                    Array.Clear(extendedkey, salt.Length, 4);
                    byte[] f = u;
                    for (int j = 1; j < iterationCount; j++)
                    {
                        u = hmac.ComputeHash(u);
                        for (int k = 0; k < f.Length; k++)
                        {
                            f[k] ^= u[k];
                        }
                    }
                    ms.Write(f, 0, f.Length);
                    Array.Clear(u, 0, u.Length);
                    Array.Clear(f, 0, f.Length);
                }
                byte[] dk = new byte[dklen];
                ms.Position = 0;
                ms.Read(dk, 0, dklen);
                ms.Position = 0;
                for (long i = 0; i < ms.Length; i++)
                {
                    ms.WriteByte(0);
                }
                Array.Clear(extendedkey, 0, extendedkey.Length);
                return dk;
            }
        }
    }
    public static string HashPassword(this string password, out byte[] salt)
    {
        salt = RandomNumberGenerator.GetBytes(HashPasswordOptions.keySize);
        var hash = Pbkdf2Sha256GetBytes(Encoding.UTF8.GetBytes(password), salt,
            HashPasswordOptions.iteration, HashPasswordOptions.keySize);
        return Convert.ToHexString(hash);
    }
    public static bool VerifyPassword(string password, string hash, byte[] salt)
    {
        var hashToCompare = Pbkdf2Sha256GetBytes(Encoding.UTF8.GetBytes(password), salt,
            HashPasswordOptions.iteration, HashPasswordOptions.keySize);

        return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
    }
    public static class HashPasswordOptions
    {
        public const int keySize = 24;
        public const int iteration = 350000;
        public static readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA3_256;
    }
}