using System.Collections.Generic;

namespace Simplic.Sms
{
    public interface ISmsService
    {
        /// <summary>
        /// Adds an sms to the sms queue
        /// </summary>
        /// <param name="number">Phone number to send an sms</param>
        /// <param name="body">The sms text</param>
        /// <returns>True if successfull</returns>
        bool Queue(string number, string body);

        /// <summary>
        /// Returns a list of queued sms
        /// </summary>
        /// <returns>a list of queued sms</returns>
        IEnumerable<SmsQueue> GetQueuedSms();

        /// <summary>
        /// Sends an sms using a provider (lox24 is default) 
        /// </summary>
        /// <param name="number">Phone number</param>
        /// <param name="body">Sms text</param>
        /// <returns>True if successfull</returns>
        bool SendSms(SmsQueue smsQueue);

        /// <summary>
        /// Saves an sms queue object in the db
        /// </summary>
        /// <param name="smsQueue">Sms</param>        
        /// <returns>True if successfull</returns>
        bool Save(SmsQueue smsQueue);

        /// <summary>
        /// Saves a list of sms queue object in the db
        /// </summary>
        /// <param name="smsQueues">List of sms object</param>        
        /// <returns>True if successfull</returns>
        bool SaveRange(IList<SmsQueue> smsQueues);
    }
}
