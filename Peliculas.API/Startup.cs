using Microsoft.EntityFrameworkCore;
using Peliculas.API.Context;
using Peliculas.API.Servicios.FilesManager;

namespace Peliculas.API
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddTransient<IFileManager, FilaManagerAzure>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("PeliculaDb")));
            services.AddControllers().AddNewtonsoftJson();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
                app.UseSwagger();
                app.UseSwaggerUI(c => { });
                app.UseDeveloperExceptionPage();
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
