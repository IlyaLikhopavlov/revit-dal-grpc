using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bimdance.Revit.Framework.RevitDocument
{
    public static class DocumentExtensions
    {
        public const string SaveChangesTransactionName = @"SaveChanges";
        
        public static void SaveChanges(this Document document, Action sync, bool isInSubTransaction = false)
        {
            if (document.IsReadOnly)
            {
                throw new InvalidOperationException($"Document {document.Title} is read only. Changes can't be saved.");
            }

            if (!document.IsModifiable && !isInSubTransaction)
            {
                using var transaction = new Transaction(document, SaveChangesTransactionName);
                try
                {
                    if (transaction.Start() == TransactionStatus.Started)
                    {
                        sync.Invoke();
                    }

                    if (TransactionStatus.Committed != transaction.Commit())
                    {
                        transaction.RollBack();
                    }
                }
                catch (Exception)
                {
                    transaction.RollBack();
                    throw;
                }
            }
            else
            {
                using var subTransaction = new SubTransaction(document);
                try
                {
                    if (subTransaction.Start() == TransactionStatus.Started)
                    {
                        sync.Invoke();
                    }

                    if (TransactionStatus.Committed != subTransaction.Commit())
                    {
                        subTransaction.RollBack();
                    }
                }
                catch (Exception)
                {
                    subTransaction.RollBack();
                    throw;
                }
            }
        }
    }
}
