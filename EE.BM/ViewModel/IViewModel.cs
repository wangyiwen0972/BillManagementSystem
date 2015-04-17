using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EE.BM.DAL;
using EE.BM.Model;
namespace EE.BM
{
    public interface IModel
    {
        bool IsVaild();
    }

    public interface IViewModel
    {
        string OutputMessage { get; set; }
        ViewModelStatus GetCurrentStatus();
        void SetCurrentStats(ViewModelStatus status);
        UserModel GetCurrentLoginUser();
    }

    public enum ViewModelStatus
    {
        NewRecord,
        Changed,
        Saved,
        NoChange,
        Invaild
    }
}
