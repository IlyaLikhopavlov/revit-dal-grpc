using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit.DAL.Utils
{
    public static class DocumentProcessing
    {
        public static void ExecuteTransaction(this Document document, Action action, string transactionName)
        {
            if (document.IsReadOnly || document.IsModifiable)
            {
                action?.Invoke();
            }
            else
            {
                using var transaction = new Transaction(document, transactionName);
                try
                {
                    if (transaction.Start() == TransactionStatus.Started)
                    {
                        action?.Invoke();
                    }

                    if (transaction.Commit() != TransactionStatus.Committed)
                    {
                        transaction.RollBack();
                    }
                }
                catch (Exception)
                {
                    if (transaction.HasStarted())
                    {
                        transaction.RollBack();
                    }

                    throw;
                }
            }
        }

        public static void ExecuteSubTransaction(this Document document, Action action)
        {
            using var subTransaction = new SubTransaction(document);
            try
            {
                if (subTransaction.Start() == TransactionStatus.Started)
                {
                    action?.Invoke();
                }

                if (subTransaction.Commit() != TransactionStatus.Committed)
                {
                    subTransaction.RollBack();
                }
            }
            catch (Exception)
            {
                if (subTransaction.HasStarted())
                {
                    subTransaction.RollBack();
                }
                throw;
            }
        }
    }
}
