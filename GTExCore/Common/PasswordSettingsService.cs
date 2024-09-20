namespace GTExCore.Common
{
    public interface IPasswordSettingsService
    {
        string PasswordForValidate { get; }
        string PasswordForValidateS { get; }
    }

    public class PasswordSettingsService : IPasswordSettingsService
    {
        public string PasswordForValidate { get; }
        public string PasswordForValidateS { get; }

        public PasswordSettingsService(IConfiguration configuration)
        {
            PasswordForValidate = configuration["PasswordSettings:PasswordForValidate"];
            PasswordForValidateS = configuration["PasswordSettings:PasswordForValidateS"];
        }
    }
}
