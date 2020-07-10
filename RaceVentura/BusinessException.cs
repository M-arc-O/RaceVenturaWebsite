using RaceVentura.Models;
using System;

namespace RaceVentura
{
    public class BusinessException: Exception
    {
        public BLErrorCodes ErrorCode { get; set; }

        public BusinessException(string message, BLErrorCodes code) : base(message)
        {
            ErrorCode = code;
        }
    }
}
