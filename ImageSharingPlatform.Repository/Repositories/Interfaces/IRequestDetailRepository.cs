﻿using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Repository.Repositories.Interfaces
{
    public interface IRequestDetailRepository : IGenericRepository<RequestDetail, Guid>
    {
        Task<IEnumerable<RequestDetail>> GetAllByRequestId(Guid requestId);
    }
}
