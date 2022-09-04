using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_ExceptionCatcher
{
    public interface IExceptionCatcher
    {
        IEnumerable<Exception> GetFlatList(Exception ex);
    }
    public class ProgramMessage : IExceptionCatcher
    {

        public IEnumerable<Exception> GetFlatList(Exception ex)
        {
            var all = new List<Exception>();
            Exception pex = ex;
            do
            {
                all.Add(pex);
                pex = ex.InnerException;
            } while (pex != null);
            return all;
        }
    }
    public class ExceptionInfo
    {
        private readonly Exception _ex;

        public List<ExceptionScopeInfo> GetExceptionScopeInfos(IExceptionCatcher catcher)
            => catcher.GetFlatList(_ex).Select(ex => new ExceptionScopeInfo(ex)).ToList();
        public ExceptionInfo(Exception ex)
        {
            _ex = ex;
        }
        public struct ActionCallInfo
        {
            public string FileName { get; }
            public int LineNumber { get; }
        }
        public struct ExceptionScopeInfo
        {
            public string Message { get; }
            public ExceptionScopeInfo(Exception ex)
            {
                this.Message = ex.Message;
            }

            
        }
        
    }
    
}
