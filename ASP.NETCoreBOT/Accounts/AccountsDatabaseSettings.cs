using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NETCoreBOT
{
    public class AccountsDatabaseSettings:IAccountDatabaseSettings
    {
        public String ConnectionString { get; set; }
        public String DatabaseName { get; set; }
    }
    public class IAccountDatabaseSettings
    {
        public String ConnectionString { get; set; }
        public String DatabaseName { get; set; }
    }
}
