using Dapper;
using Simplic.Configuration;
using Simplic.Sms.Service.Provider;
using Simplic.Sms.Service.Provider.Lox24;
using Simplic.Sql;
using System;
using System.Collections.Generic;

namespace Simplic.Sms.Service
{
    public class SmsService : ISmsService
    {
        #region Private Consts
        private const string TableName = "Sms_Queue";
        private const string SettingsPluginName = "IconChecksum";
        #endregion

        private readonly ISmsProvider smsProvider;
        private readonly ISqlService sqlService;

        public SmsService(ISqlService sqlService, IConfigurationService configurationService)
        {
            smsProvider = new Lox24Provider(configurationService);
            this.sqlService = sqlService;
        }

        /// <summary>
        /// Adds an sms to the sms queue
        /// </summary>
        /// <param name="nummer">Phone number to send an sms</param>
        /// <param name="body">The sms text</param>
        /// <returns>True if successfull</returns>
        public bool Queue(string number, string body)
        {
            return sqlService.OpenConnection((connection) =>
            {
               var affectedRows = connection.Execute($"INSERT INTO {TableName} (Number, Body, Status) " +
               "VALUES (:Number, :Body, :Status)", new { Number = number, Body = body, Status = SmsStatus.InQueue });

               return affectedRows > 0;
            });
        }

        /// <summary>
        /// Returns a list of queued sms
        /// </summary>
        /// <returns>a list of queued sms</returns>
        public IEnumerable<SmsQueue> GetQueuedSms()
        {
            return sqlService.OpenConnection((connection) =>
            {
                return connection.Query<SmsQueue>($"SELECT * FROM {TableName} WHERE Status = :Status ORDER BY Id", 
                    new { Status = SmsStatus.InQueue });
            });
        }

        /// <summary>
        /// Sends an sms using a provider (lox24 is default) 
        /// </summary>
        /// <param name="number">Phone number</param>
        /// <param name="body">Sms text</param>
        /// <returns>True if successfull</returns>
        public bool SendSms(SmsQueue smsQueue)
        {
            var result = smsProvider.SendSms(smsQueue.Number, smsQueue.Body);
            if (result == null) return false;

            var resultLox24 = result as Lox24Result;
            if (resultLox24 != null)
            {
                if (resultLox24.StatusCode == Lox24ResultStatus.SmsSuccess ||
                    resultLox24.StatusCode == Lox24ResultStatus.SuccessfulQuery ||
                    resultLox24.StatusCode == Lox24ResultStatus.CommandExecuted)
                {
                    smsQueue.SentTime = DateTime.Now;
                    smsQueue.Status = SmsStatus.Sent;
                    smsQueue.ProviderResponse = resultLox24.ResultText;

                    Save(smsQueue);

                    return true;
                }
                else
                {                    
                    smsQueue.Status = SmsStatus.Failed;
                    smsQueue.ProviderResponse = resultLox24.ResultText;

                    Save(smsQueue);

                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Saves an sms queue object in the db
        /// </summary>
        /// <param name="smsQueue">Sms</param>        
        /// <returns>True if successfull</returns>
        public bool Save(SmsQueue smsQueue)
        {
            return sqlService.OpenConnection((connection) =>
            {
                var affectedRows = connection.Execute($"INSERT INTO {TableName} (Id, Number, Body, Status, ProviderResponse, SentTime) "
                     + " ON EXISTING UPDATE VALUES (:Id, :Number, :Body, :Status, :ProviderResponse, :SentTime)", smsQueue);

                return affectedRows > 0;
            });
        }

        /// <summary>
        /// Saves a list of sms queue object in the db
        /// </summary>
        /// <param name="smsQueues">List of sms object</param>        
        /// <returns>True if successfull</returns>
        public bool SaveRange(IList<SmsQueue> smsQueues)
        {
            return sqlService.OpenConnection((connection) =>
            {
                var affectedRows = connection.Execute($"INSERT INTO {TableName} (Id, Number, Body, Status, ProviderResponse, SentTime) "
                     + " ON EXISTING UPDATE VALUES (:Id, :Number, :Body, :Status, :ProviderResponse, :SentTime)", smsQueues);

                return affectedRows > 0;
            });
        }
    }
}
