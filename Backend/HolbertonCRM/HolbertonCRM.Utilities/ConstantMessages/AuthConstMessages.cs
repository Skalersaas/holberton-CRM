using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolbertonCRM.Utilities.ConstantMessages
{
    public static class AuthMessages
    {
        // User Management
        public const string UserNotFound = "User not found.";
        public const string UserAlreadyExists = "User already exists.";
        public const string AccountLocked = "Account is locked.";
        public const string AccountDisabled = "Account is disabled.";
        public const string EmailNotVerified = "Email is not verified.";
        public const string EmailVerificationSent = "Email verification link has been sent.";
        public const string EmailVerificationSuccessful = "Email verification successful.";
        public const string EmailVerificationFailed = "Email verification failed.";

        // General Errors
        public const string InternalServerError = "An internal server error occurred.";
        public const string NotFound = "Requested resource not found.";
        public const string AccessDenied = "Access denied.";
        public const string UnauthorizedAccess = "Unauthorized access.";
        public const string BadRequest = "Bad request.";
        public const string OperationSuccessful = "Operation completed successfully.";
        public const string OperationFailed = "Operation failed.";
        public const string InvalidModelState = "Invalid model state.";

        // Authentication & Authorization
        public const string InvalidLoginAttempt = "Invalid login attempt.";
        public const string InvalidUsernameOrPassword = "Invalid username or password.";
        public const string LoginSuccessful = "Login successful.";
        public const string SessionExpired = "Your session has expired. Please log in again.";
        public const string TokenExpired = "Token has expired.";
        public const string TokenInvalid = "Token is invalid.";
        public const string LoginAttemptLimitExceeded = "Login attempt limit exceeded. Please try again later.";
        public const string LogoutSuccessful = "Logout successful.";
        public const string TwoFactorAuthenticationRequired = "Two-factor authentication is required.";
        public const string TwoFactorAuthenticationFailed = "Two-factor authentication failed.";
        public const string TwoFactorAuthenticationSuccessful = "Two-factor authentication successful.";

        // Password Management
        public const string PasswordChangeSuccessful = "Password changed successfully.";
        public const string PasswordChangeFailed = "Password change failed.";
        public const string InvalidPasswordChangeRequest = "Invalid change password request.";
        public const string PasswordResetSuccessful = "Password reset successfully.";
        public const string PasswordResetFailed = "Password reset failed.";
        public const string InvalidPasswordResetRequest = "Invalid password reset request.";
        public const string PasswordMismatch = "Passwords do not match.";
        public const string WeakPassword = "The password does not meet the complexity requirements.";
        public const string PasswordTooShort = "The password is too short.";
        public const string PasswordTooLong = "The password is too long.";
        public const string PasswordExpired = "Password has expired.";

        // Registration
        public const string RegistrationSuccessful = "Registration successful.";
        public const string RegistrationFailed = "Registration failed.";

        // Email Notifications
        public const string EmailSent = "Email has been sent successfully.";
        public const string EmailFailedToSend = "Failed to send the email.";
        public const string InvalidEmailFormat = "Invalid email format.";

        // Email Body Messages
        public const string ResetPasswordTitle = "Reset Password";
        public const string ResetPasswordMessage = "To reset your password, please follow the link below:";
        public const string VerifyEmailTitle = "Email Verification";
        public const string VerifyEmailMessage = "OTP for verification: ";
        public const string DefaultEmailTitle = "Confirmation Email";
        public const string DefaultEmailMessage = "This email has been sent for informational purposes.";

        // File Operations
        public const string FileUploadSuccessful = "File uploaded successfully.";
        public const string FileUploadFailed = "File upload failed.";
        public const string FileNotFound = "File not found.";
        public const string FileDownloadSuccessful = "File downloaded successfully.";
        public const string FileDownloadFailed = "File download failed.";

        // Database Operations
        public const string RecordNotFound = "Record not found.";
        public const string RecordCreatedSuccessfully = "Record created successfully.";
        public const string RecordUpdatedSuccessfully = "Record updated successfully.";
        public const string RecordDeletedSuccessfully = "Record deleted successfully.";
        public const string RecordCreationFailed = "Failed to create the record.";
        public const string RecordUpdateFailed = "Failed to update the record.";
        public const string RecordDeletionFailed = "Failed to delete the record.";

        // Custom Messages
        public const string FeatureUnavailable = "This feature is currently unavailable.";
        public const string InvalidInput = "Invalid input.";
        public const string ActionNotAllowed = "You are not allowed to perform this action.";
        public const string MaintenanceMode = "The system is under maintenance. Please try again later.";
    }
}
