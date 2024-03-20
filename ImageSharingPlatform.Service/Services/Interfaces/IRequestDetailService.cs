using ImageSharingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Service.Services.Interfaces
{
    public interface IRequestDetailService
    {
        Task<IEnumerable<RequestDetail>> GetRequestDetailByRequetIdAsync(Guid requestId);
        Task<RequestDetail> AddRequestDetailAsync(RequestDetail requestDetail);
    }
}
