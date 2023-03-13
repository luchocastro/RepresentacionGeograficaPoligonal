using AutoMapper;
using Hexagon.Model.Models;
using Hexagon.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
 using System.Text;

namespace Hexagon.Model.FileDataManager
{
    public class UserFileDataManager<G, T> : FileDataManager<G, T>, IForFile<T> where T : User where G : UserDTO
    {

        public UserFileDataManager(IMapper Mapper, IConfiguration IConfiguration, IFileDataManagerOptions IFileDataManagerOptions) : base(Mapper, IConfiguration, IFileDataManagerOptions)
        {
             
        }
        public override G Get(string UserName)
        {
            G User;
            try
            {
                T UserToGet = (T) new User() ;
                UserToGet.Name = UserName;
                UserToGet.ParentID = Path.Combine(UserToGet.GetType().Name, UserToGet.Name);
                User = base.Get(GenerateFullID(UserToGet));
                
            }
            catch (Exception ex)
            {
                if ((ex.GetType()) == typeof(FileNotFoundException))
                {
                    return null;
                }
                else
                    throw ex;

            }

            return User;

        }
        public override G Add(T User)
        {
            User.ID = Path.Combine(User.Name );
            User.ParentID = Path.Combine(User.GetType().Name, User.Name);
            return base.Add(User);

        }
    }
}
