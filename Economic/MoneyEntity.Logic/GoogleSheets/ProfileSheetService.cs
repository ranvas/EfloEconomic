using GoogleSheet.Abstractions;
using GoogleSheet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Logic.GoogleSheets
{
    public class ProfileSheetService : SheetServiceBase<ProfileSheet>
    {
        public ProfileSheetService(ISheetAdapter adapter) : base(adapter)
        {
        }
        protected override string SpreadSheetId { get; set; } = "1bfsiGsXphy7NzC6oxVfnEXJmHmZYubSMjdHR6iwa4Hc";
        protected override GoogleSheetRange Range { get; set; } =
            new GoogleSheetRange
            {
                List = "Profiles",
                StartColumn = "A",
                StartRow = 1,
                EndRow = 10000,
                EndColumn = "H"
            };
    }
}
