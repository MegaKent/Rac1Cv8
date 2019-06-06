
namespace Rac1Cv8
{
    public class Rule
    {
        public string UID { get; private set; }
        public ObjectTypeEnum ObjectType { get; private set; }
        public string InfobaseName { get; private set; }
        public RuleTypeEnum RuleType { get; private set; }
        public string ApplicationExt { get; private set; }
        public int Pririty { get; private set; }

        public enum ObjectTypeEnum
        {
            All,
            ClientTestingService,
            Connection,
            SessionDataService,
            DataEditLockService,
            JobService,
            ExternalDataSourceXMLAService,
            ExternalSessionManagerService,
            EventLogService,
            TimestampService,
            AuxiliaryService,
            ExternalDataSourceODBCService,
            OpenID2ProviderContextService,
            SessionReuseService,
            TransactionLockService,
            LicenseService,
            FulltextSearchService,
            SettingsService,
            DataBaseConfigurationUpdateService,
            DatabaseTableNumberingService
        }
        public enum RuleTypeEnum
        {
            Auto,
            Always,
            Never
        }

        public Rule()
        {

        }

        public Rule(string[] props)
        {
            InitializeProperties(props);
        }

        private void InitializeProperties(string[] props)
        {
            UID             = props[0];
            ObjectType      = GetObjectType(props[1]);
            InfobaseName    = props[2];
            RuleType        = GetRuleType(props[3]);
            ApplicationExt  = props[4];
            Pririty         = int.TryParse(props[5], out int _Priority) ? _Priority : -1;
        }

        private RuleTypeEnum GetRuleType(string str)
        {
            switch (str)
            {
                case "always" : return RuleTypeEnum.Always;
                case "never"  : return RuleTypeEnum.Never;

                default       : return RuleTypeEnum.Auto;
            }
        }

        private ObjectTypeEnum GetObjectType(string str)
        {
            switch (str)
            {
                case "ClientTestingService"              : return ObjectTypeEnum.ClientTestingService;
                case "Connection"                        : return ObjectTypeEnum.Connection;
                case "SessionDataService"                : return ObjectTypeEnum.SessionDataService;
                case "DataEditLockService"               : return ObjectTypeEnum.DataEditLockService;
                case "JobService"                        : return ObjectTypeEnum.JobService;
                case "ExternalDataSourceXMLAService"     : return ObjectTypeEnum.ExternalDataSourceXMLAService;
                case "ExternalSessionManagerService"     : return ObjectTypeEnum.ExternalSessionManagerService;
                case "EventLogService"                   : return ObjectTypeEnum.EventLogService;
                case "TimestampService"                  : return ObjectTypeEnum.TimestampService;
                case "AuxiliaryService"                  : return ObjectTypeEnum.AuxiliaryService;
                case "ExternalDataSourceODBCService"     : return ObjectTypeEnum.ExternalDataSourceODBCService;
                case "OpenID2ProviderContextService"     : return ObjectTypeEnum.OpenID2ProviderContextService;
                case "SessionReuseService"               : return ObjectTypeEnum.SessionReuseService;
                case "TransactionLockService"            : return ObjectTypeEnum.TransactionLockService;
                case "LicenseService"                    : return ObjectTypeEnum.LicenseService;
                case "FulltextSearchService"             : return ObjectTypeEnum.FulltextSearchService;
                case "SettingsService"                   : return ObjectTypeEnum.SettingsService;
                case "DataBaseConfigurationUpdateService": return ObjectTypeEnum.DataBaseConfigurationUpdateService;
                case "DatabaseTableNumberingService"     : return ObjectTypeEnum.DatabaseTableNumberingService;

                default: return ObjectTypeEnum.All;
            }
        }
    }
}
