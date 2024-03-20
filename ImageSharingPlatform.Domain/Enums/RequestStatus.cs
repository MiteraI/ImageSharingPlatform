using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Domain.Enums
{
    public enum RequestStatus
    {
        PROCESSING, USER_ACCEPTED, ARTIST_ACCEPTED, ACCEPTED, REJECTED, CANCELLED, UPLOADED, SUCCESS 
    }
}
