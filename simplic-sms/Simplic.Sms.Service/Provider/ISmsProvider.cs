namespace Simplic.Sms.Service.Provider
{
    public interface ISmsProvider
    {
        /// <summary>
        /// Sends an sms using a provider
        /// </summary>
        /// <param name="number">Phone number</param>
        /// <param name="body">Sms text</param>
        /// <returns>True if successfull</returns>
        object SendSms(string number, string body);
    }
}