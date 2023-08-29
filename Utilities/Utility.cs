using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_Console.Utilities
{
    public class Utility
    {
        // Encode password
        public static string EncodePassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // verify password
        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        
        }

        // validate one or more string input is not empty
        public static bool ValidateStringInput(params string[] inputs)
        {
            foreach (var input in inputs)
            {
                if (string.IsNullOrEmpty(input))
                {
                    return false;
                }
            }
            return true;
        }

        // validate input range
        public static bool ValidateRange(int input, int min, int max)
        {
            if (input < min || input > max)
            {
                return false;
            }
            return true;
        }
    }
}