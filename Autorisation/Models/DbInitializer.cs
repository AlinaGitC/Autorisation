using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autorisation.Models
{
    public class DbInitializer
    {
        public static void Initialize()
        {
            /*using (var context = new AppDbContext())
            {
                context.Database.EnsureCreated();

                // Добавляем роль инспектора, если её нет
                if (!context.Roles.Any(r => r.Name == "Inspector"))
                {
                    context.Roles.Add(new Role { Name = "Inspector" });
                    context.SaveChanges();
                }

                // Добавляем пользователя inspector, если его нет
                if (!context.Users.Any(u => u.Login == "inspector"))
                {
                    var inspectorRole = context.Roles.First(r => r.Name == "Inspector");
                    context.Users.Add(new Userr
                    {
                        Login = "inspector",
                        Password = "inspector",
                        ID_Role = inspectorRole.ID
                    });
                    context.SaveChanges();
                }
            }*/
        }
    }
}
