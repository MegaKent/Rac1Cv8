using System;
using System.Text;

namespace Rac1Cv8
{
    public class RacCmdBuilder
    {
        public static string GetClusterCmd(string ConnStr)
        {
            StringBuilder cmd = new StringBuilder();
            cmd.AppendFormat("{0} cluster list", ConnStr);

            return cmd.ToString();
        }

        public static string ClusterAuthCmd(string ConnStr, string ClusterUID, string ClusterUser, string ClusterPwd)
        {
            StringBuilder cmd = new StringBuilder();

            cmd.AppendFormat("{0} cluster admin list --cluster={1} {2}",
                ConnStr,
                ClusterUID,
                (ClusterUser == string.Empty) ? "" : "--cluster-user=" + ClusterUser + " --cluster-pwd=" + ClusterPwd
                );

            return cmd.ToString();
        }

        public static string GetSessionsCmd(string ConnStr, string ClusterUID, string ClusterUser, string ClusterPwd)
        {
            StringBuilder cmd = new StringBuilder();

            cmd.AppendFormat("{0} session list --cluster={1} {2}",
                ConnStr,
                ClusterUID,
                (ClusterUser == string.Empty) ? "" : "--cluster-user=" + ClusterUser + " --cluster-pwd=" + ClusterPwd
                );

            return cmd.ToString();
        }

        public static string GetLicensesCmd(string ConnStr, string ClusterUID, string ClusterUser, string ClusterPwd)
        {
            StringBuilder cmd = new StringBuilder();

            cmd.AppendFormat("{0} session list --licenses --cluster={1} {2}",
                ConnStr,
                ClusterUID,
                (ClusterUser == string.Empty) ? "" : "--cluster-user=" + ClusterUser + " --cluster-pwd=" + ClusterPwd
                );

            return cmd.ToString();
        }

        public static string TerminateSessionCmd(
            string ConnStr,
            string ClusterUID,
            string ClusterUser,
            string ClusterPwd,
            string SessionUID)
        {
            StringBuilder cmd = new StringBuilder();

            cmd.AppendFormat("{0} session terminate --session={1} --cluster={2} {3}",
                    ConnStr,
                    SessionUID,
                    ClusterUID,
                    (ClusterUser == string.Empty) ? "" : "--cluster-user=" + ClusterUser + " --cluster-pwd=" + ClusterPwd
                    );

            return cmd.ToString();
        }

        public static string GetInfoBaseListCmd(string ConnStr, string ClusterUID, string ClusterUser, string ClusterPwd)
        {
            StringBuilder cmd = new StringBuilder();

            cmd.AppendFormat("{0} infobase summary list --cluster={1} {2}",
                ConnStr,
                ClusterUID,
                (ClusterUser == string.Empty) ? "" : "--cluster-user=" + ClusterUser + " --cluster-pwd=" + ClusterPwd
                );

            return cmd.ToString();
        }

        public static string InfoBaseAuthCmd(
            string ConnStr,
            string ClusterUID,
            string ClusterUser,
            string ClusterPwd,
            string InfoBaseUID,
            string InfoBaseUser,
            string InfoBasePwd)
        {
            StringBuilder cmd = new StringBuilder();

            cmd.AppendFormat("{0} infobase info --cluster={1} --infobase={2} {3} {4}",
                ConnStr,
                ClusterUID,
                InfoBaseUID,
                (ClusterUser == string.Empty) ? "" : "--cluster-user=" + ClusterUser + " --cluster-pwd=" + ClusterPwd,
                (InfoBaseUser == string.Empty) ? "" : "--infobase-user=\"" + InfoBaseUser + "\" --infobase-pwd=" + InfoBasePwd
                );

            return cmd.ToString();
        }

        public static string LockInfoBaseCmd(
            string ConnStr,
            string ClusterUID,
            string ClusterUser,
            string ClusterPwd,
            string InfoBaseUID,
            string InfoBaseUser,
            string InfoBasePwd,
            DateTime DeniedFrom,
            DateTime DeniedTo,
            string DeniedMessage,
            string PermissionCode,
            bool ScheduledJobsDeny,
            bool SessionDeny)
        {
            StringBuilder cmd = new StringBuilder();

            cmd.AppendFormat("{0} infobase update --cluster={1} --infobase={2} --sessions-deny={3} --denied-from={4} --denied-to={5} --denied-message=\"{6}\" --permission-code={7} --scheduled-jobs-deny={8} {9} {10}",
                ConnStr,
                ClusterUID,
                InfoBaseUID,
                SessionDeny ? "on" : "off",
                (DeniedFrom == DateTime.MinValue) ? "" : DeniedFrom.ToString("yyyy-MM-ddTHH:mm:ss"),
                (DeniedTo == DateTime.MinValue) ? "" : DeniedTo.ToString("yyyy-MM-ddTHH:mm:ss"),
                DeniedMessage,
                PermissionCode,
                (ScheduledJobsDeny) ? "on" : "off",
                (ClusterUser == string.Empty) ? "" : "--cluster-user=" + ClusterUser + " --cluster-pwd=" + ClusterPwd,
                (InfoBaseUser == string.Empty) ? "" : "--infobase-user=\"" + InfoBaseUser + "\" --infobase-pwd=" + InfoBasePwd
                );

            return cmd.ToString();
        }

        public static string UnLockInfoBaseCmd(
            string ConnStr,
            string ClusterUID,
            string ClusterUser,
            string ClusterPwd,
            string InfoBaseUID,
            string InfoBaseUser,
            string InfoBasePwd,
            bool ScheduledJobsAllow)
        {
            StringBuilder cmd = new StringBuilder();

            cmd.AppendFormat("{0} infobase update --cluster={1} --infobase={2} --sessions-deny=off --denied-from= --denied-to= --denied-message= --permission-code= --scheduled-jobs-deny={3} {4} {5}",
                ConnStr,
                ClusterUID,
                InfoBaseUID,
                (ScheduledJobsAllow) ? "off" : "on",
                (ClusterUser == string.Empty) ? "" : "--cluster-user=" + ClusterUser + " --cluster-pwd=" + ClusterPwd,
                (InfoBaseUser == string.Empty) ? "" : "--infobase-user=\"" + InfoBaseUser + "\" --infobase-pwd=" + InfoBasePwd
                );

            return cmd.ToString();
        }

        public static string DeleteInfoBaseCmd(
            string ConnStr,
            string ClusterUID,
            string ClusterUser,
            string ClusterPwd,
            string InfoBaseUID,
            string InfoBaseUser,
            string InfoBasePwd,
            bool DropDataBase,
            bool ClearDatabase)
        {
            StringBuilder cmd = new StringBuilder();

            if (DropDataBase)
            {
                ClearDatabase = false;
            }

            cmd.AppendFormat("{0} infobase drop --infobase={1} --cluster={2} {3} {4} {5}",
                ConnStr,
                InfoBaseUID,
                ClusterUID,
                (ClusterUser == string.Empty) ? "" : "--cluster-user=" + ClusterUser + " --cluster-pwd=" + ClusterPwd,
                (!DropDataBase ) ? "" : "--drop-database --infobase-user=\"" + InfoBaseUser + "\" --infobase-pwd=" + InfoBasePwd,
                (!ClearDatabase) ? "" : "--clear-database --infobase-user=\"" + InfoBaseUser + "\" --infobase-pwd=" + InfoBasePwd
                );

            return cmd.ToString();
        }

        public static string CreateInfoBaseCmd(
            string ConnStr,
            string ClusterUID,
            string ClusterUser,
            string ClusterPwd,
            string[] InfoBaseProps
            )
        {
            StringBuilder cmd = new StringBuilder();

            cmd.AppendFormat("{0} infobase create --cluster={1} --name={2} --dbms={3} --db-server={4} --db-name={5} --locale={6} --date-offset={7} --security-level={8} --scheduled-jobs-deny={9} --license-distribution={10} {11} {12} {13}",
                ConnStr,
                ClusterUID,
                InfoBaseProps[0],
                InfoBaseProps[1],
                InfoBaseProps[2],
                InfoBaseProps[3],
                InfoBaseProps[5],
                InfoBaseProps[9],
                InfoBaseProps[10],
                InfoBaseProps[11],
                InfoBaseProps[12],
                (InfoBaseProps[6] == string.Empty) ? "" : "--db-user=" + InfoBaseProps[6] + " ----db-pwd=" + InfoBaseProps[7],
                (InfoBaseProps[4] == "no") ? "" : "--create-database",
                (InfoBaseProps[8] == string.Empty) ? "" : "--descr=\""+ InfoBaseProps[8] + "\"",
                (ClusterUser == string.Empty) ? "" : "--cluster-user=" + ClusterUser + " --cluster-pwd=" + ClusterPwd
                );

            return cmd.ToString();
        }

        public static string UnLockInfoBaseEasyCmd(
             string ConnStr,
             string ClusterUID,
             string ClusterUser,
             string ClusterPwd,
             string InfoBaseUID,
             string InfoBaseUser,
             string InfoBasePwd,
             bool ScheduledJobsAllow)
        {
            StringBuilder cmd = new StringBuilder();

            cmd.AppendFormat("{0} infobase update --cluster={1} --infobase={2} --sessions-deny=off {3} {4}",
                ConnStr,
                ClusterUID,
                InfoBaseUID,
                (ClusterUser == string.Empty) ? "" : "--cluster-user=" + ClusterUser + " --cluster-pwd=" + ClusterPwd,
                (InfoBaseUser == string.Empty) ? "" : "--infobase-user=\"" + InfoBaseUser + "\" --infobase-pwd=" + InfoBasePwd
                );

            return cmd.ToString();
        }

        public static string GetProcessesCmd(string ConnStr, string ClusterUID, string ClusterUser, string ClusterPwd)
        {
            StringBuilder cmd = new StringBuilder();

            cmd.AppendFormat("{0} process list --cluster={1} {2}",
                ConnStr,
                ClusterUID,
                (ClusterUser == string.Empty) ? "" : "--cluster-user=" + ClusterUser + " --cluster-pwd=" + ClusterPwd
                );

            return cmd.ToString();
        }

        public static string GetServerListCmd(string ConnStr, string ClusterUID, string ClusterUser, string ClusterPwd)
        {
            StringBuilder cmd = new StringBuilder();

            cmd.AppendFormat("{0} server list --cluster={1} {2}",
                ConnStr,
                ClusterUID,
                (ClusterUser == string.Empty) ? "" : "--cluster-user=" + ClusterUser + " --cluster-pwd=" + ClusterPwd
                );

            return cmd.ToString();
        }

        public static string GetRuleListCmd(string ConnStr, string ClusterUID, string ClusterUser, string ClusterPwd, string ServerUID)
        {
            StringBuilder cmd = new StringBuilder();

            cmd.AppendFormat("{0} rule list --server={1} --cluster={2} {3}",
                ConnStr,
                ServerUID,
                ClusterUID,
                (ClusterUser == string.Empty) ? "" : "--cluster-user=" + ClusterUser + " --cluster-pwd=" + ClusterPwd
                );

            return cmd.ToString();
        }

        public static string GetClusterRecyclingCmd(string ConnStr, string ClusterUID, string AgentUser, string AgentPwd, int seconds)
        {

            StringBuilder cmd = new StringBuilder();

            cmd.AppendFormat("{0} cluster update --cluster={1} --lifetime-limit={2} {3}",
                ConnStr,
                ClusterUID,
                seconds.ToString(),
                (AgentUser == string.Empty) ? "" : "--agent-user=" + AgentUser + " --agent-pwd=" + AgentPwd
                );

            return cmd.ToString();
        }
    }
}
