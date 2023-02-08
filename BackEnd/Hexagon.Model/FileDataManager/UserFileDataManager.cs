using AutoMapper;
using Hexagon.Model.Models;
using Hexagon.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
 using System.Text;

namespace Hexagon.Model.FileDataManager
{
    public class UserFileDataManager<G, T> : FileDataManager<G, T>, IForFile<T> where T : User where G : UserDTO
    {

        public UserFileDataManager(IMapper Mapper) : base(Mapper)
        {
             
        }
        public override G Get(string UserName)
        {
            G User;
            try
            {

                User = base.Get(Path.Combine(UserName ));
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
            return base.Add(User);

        }
    }
}
