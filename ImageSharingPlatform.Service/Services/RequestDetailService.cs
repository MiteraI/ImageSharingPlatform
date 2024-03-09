using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Service.Services
{
    public class RequestDetailService : IRequestDetailService
    {
        private readonly IRequestDetailRepository _requestDetailRepository;
        public RequestDetailService(IRequestDetailRepository requestDetailRepository)
        {
            _requestDetailRepository = requestDetailRepository;
        }

        public async Task<RequestDetail> AddRequestDetailAsync(RequestDetail requestDetail)
        {
            var newRequestDetail = _requestDetailRepository.Add(requestDetail);
            await _requestDetailRepository.SaveChangesAsync();
            return newRequestDetail;
        }

        public async Task<IEnumerable<RequestDetail>> GetRequestDetailByRequetIdAsync(Guid requestId)
        {
            return await _requestDetailRepository.GetAllByRequestId(requestId);
        }
    }
}
