﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CT.Backend.Shared
{
    public static class TokenGenerator
    {
        public static string GenerateToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            return token
                .Replace("/", ""); // table storage don't like '/'
        }
    }
}
