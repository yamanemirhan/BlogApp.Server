namespace BlogApp.Application.DTOs.Requests
{
    public record ChangePasswordRequestDto(
         string OldPassword,   
         string NewPassword  
     );
}
