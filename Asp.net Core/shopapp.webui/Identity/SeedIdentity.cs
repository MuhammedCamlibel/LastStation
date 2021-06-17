using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace shopapp.webui.Identity
{
    public class SeedIdentity
    {
        
        public static async Task Seed(UserManager<User> userManager ,RoleManager<IdentityRole> roleManager,IConfiguration configuration)
        {
            var username  = configuration["Data:AdminUser:username"]; // appsettings.json dan bilgi almak için kullanılır 
            var email  = configuration["Data:AdminUser:email"];
            var password  = configuration["Data:AdminUser:password"];
            var role  = configuration["Data:AdminUser:role"];

            if(await userManager.FindByNameAsync(username)==null) // böyle bir kullanıcı adı var mı diye sorduk 
            {
                await roleManager.CreateAsync(new IdentityRole(role)); // rol oluşturduk

                var user = new User(){ // kullanıcı bilgilerini girdik
                   
                   UserName = username,
                   Email = email,
                   FirstName ="Admin",
                   LastName = "Admin",
                   EmailConfirmed = true,

                };

                var result = await userManager.CreateAsync(user,password); // kullanıcı oluşturduk
                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user,role); // kullanıcıya rolü ekledik
                }
            }
        }
    }
}