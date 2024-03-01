using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace profe.webui.Data.Context
{
	public class ApplicationDbContextFactory : DesignTimeDbContextFactory<ApplicationDbContext>
    {
        protected override ApplicationDbContext CreateNewInstance(DbContextOptions<ApplicationDbContext> options)
        {
            //DbContext'in her isteğinde yeniden oluşturulmasını sağlayarak DbContext'in kapsamı ve yaşam döngüsü üzerinde daha fazla kontrol sağlar.

            return new ApplicationDbContext(options);
        }
    }
}

