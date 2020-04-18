using Adventure4You.Models;
using System;

namespace Adventure4You
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
