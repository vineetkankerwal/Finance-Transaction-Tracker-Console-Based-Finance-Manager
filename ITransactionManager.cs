using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finane_Transaction_Manager
{
    public interface ITransactionManager
    {
       
        public void  Add();
        public void Update();
        public void DisplayAllRecord(List<FinanceTransaction> transaction);
        public void DisplayByID(List<FinanceTransaction> transaction);
        public void DeleteRecord(List<FinanceTransaction> transaction);

    }
}
