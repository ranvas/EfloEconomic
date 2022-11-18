using GoogleSheet.Abstractions;
using GoogleSheet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Logic.GoogleSheets
{
    public class MineSheetService : SheetServiceBase<MineSheet>
    {
        public MineSheetService(ISheetAdapter adapter) : base(adapter)
        {
        }
        protected override string SpreadSheetId { get; set; } = "1bfsiGsXphy7NzC6oxVfnEXJmHmZYubSMjdHR6iwa4Hc";
        protected override GoogleSheetRange Range { get; set; } =
            new GoogleSheetRange
            {
                List = "Mines",
                StartColumn = "A",
                StartRow = 1,
                EndRow = 10000,
                EndColumn = "F"
            };
    }
}
