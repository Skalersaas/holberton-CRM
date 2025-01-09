using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolbertonCRM.Utilities.Enums
{
    public enum UserRegistrationResult
    {
        Success,       // User registration was successful
        InvalidInput,  // The input data provided for registration is invalid
        Failed,        // User registration failed due to an unexpected error
    }
    
}
