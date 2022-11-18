using GoogleSheet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Logic.GoogleSheets
{
    public class GoogleSheetDataManager
    {
        public ProfileSheetService ProfileSheet { get; set; }
        public MineSheetService MineSheet { get; set; }
        public GoogleSheetDataManager()
        {
            var adapter = new SheetAdapter();
            ProfileSheet = new(adapter);
            MineSheet = new(adapter);
        }

    }
}
