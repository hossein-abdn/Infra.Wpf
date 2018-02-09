using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;

namespace Infra.Wpf.Business
{
    public class Logger
    {
        public List<ILogInfo> LogList { get; set; }

        public Logger(string connectionStringName)
        {
            LogList = new List<ILogInfo>();
            ConfigLogger(connectionStringName);
        }

        private void ConfigLogger(string connectionStringName)
        {
            if (LogManager.Configuration != null)
                return;

            var config = new LoggingConfiguration();
            var dbTarget = new DatabaseTarget("database");
            config.AddTarget("database", dbTarget);

            config.Variables["userId"] = "";
            config.Variables["callsite"] = "";
            config.Variables["logtype"] = "";

            dbTarget.ConnectionStringName = connectionStringName;
            dbTarget.CommandText = @"insert into dbo.Logs ([CallSite],[Level],[Type],[Message],[Exception],[PersianDate],[UserId]) values (@callSite,@level,@logtype,@message,@exception,@persiandate,@userid)";
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@callSite", "${var:callsite"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@level", "${level}"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@logtype", "${var:logtype"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@message", "${message}"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@exception", "${exception:format=ShortType,Message,StackTrace:innerFormat=ShortType,Message,StackTrace:maxInnerExceptionLevel=10:separator= -> "));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@persiandate", "${date:format=yyyy-MM-dd HH\\:mm\\:ss:culture=fa-IR}"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@userid", "${var:userId}"));

            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, dbTarget));
            LogManager.Configuration = config;
            LogManager.KeepVariablesOnReload = true;
        }

        public void LogPendingList()
        {
            foreach (var item in LogList)
                Log(item);

            LogList?.Clear();
        }

        public void Log(ILogInfo logInfo)
        {
            if (logInfo == null)
                return;

            NLog.Logger logger = LogManager.GetCurrentClassLogger();

            LogManager.Configuration.Variables["userid"] = logInfo.UserId.ToString();
            LogManager.Configuration.Variables["callsite"] = logInfo.CallSite;
            LogManager.Configuration.Variables["logtype"] = logInfo.LogType.ToString();

            if (logInfo.Exception == null)
                logger.Info(logInfo.Message);
            else
                logger.Error(logInfo.Exception, logInfo.Message);
        }

        public void Log(Exception ex, string callSite, int userId = 0)
        {
            var logInfo = new LogInfo()
            {
                Message = "خطا",
                CallSite = callSite,
                Exception = ex,
                UserId = userId,
                LogType = LogType.Error
            };

            Log(logInfo);
        }
    }
}
