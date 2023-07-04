using EBilets.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EBilets.Services.Interface
{
    public interface IUserService
    {
        public List<UserFromFileDto> ReadUsersFromExcelFile(string fileName);

    }
}
