using System;
using System.Threading.Tasks;


namespace RevStack.Admin
{
    public interface IAdminReportService
    {
        AdminReportModel Get();
        Task<AdminReportModel> GetAsync();
    }

    public interface IAdminUserReportService
    {
        AdminReportModel Get();
        Task<AdminReportModel> GetAsync();
    }
}