namespace Foundations.Security
{
    public interface IImpersonationInfo
    {
        string Domain {get; set; }
        string UserName {get; set; }
        string Password {get; set; }
        LogonType LogonType {get; set; }
    }
}