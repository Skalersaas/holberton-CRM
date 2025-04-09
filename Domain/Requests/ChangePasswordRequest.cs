namespace Domain.Requests
{
    public record ChangePasswordRequest(string Login, string OldPassword, string NewPassword);
}
