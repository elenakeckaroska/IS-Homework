using EBilets.Domain.DTO;
using EBilets.Services.Interface;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EBilets.Services.Implementation
{
    public class UserService : IUserService
    {
        public List<UserFromFileDto> ReadUsersFromExcelFile(string fileName)
        {
            string pathToFile = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            List<UserFromFileDto> userList = new List<UserFromFileDto>();

            using (var stream = System.IO.File.Open(pathToFile, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        userList.Add(new UserFromFileDto
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            Role = reader.GetValue(2).ToString()                        
                        });
                    }
                }
            }

            return userList;
        }

    }
}
