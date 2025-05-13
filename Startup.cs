using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
 
using System.Globalization;
using System.Net.WebSockets;
using System.Xml.Serialization;
  
using MotorMemo.Models.Context;
using MotorMemo.ReportModels;
using MotorMemo.Models.Procedures;


namespace MotorMemo
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        public string? dbname { get; set; } = "motormemo.db";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddMvc();
            services.AddMemoryCache();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

           

            services.AddDbContext<MotorMemoDbContext>(options =>
            {
                string? connstring = Configuration.GetConnectionString("MotormemoConnection");
                if (connstring != null)
                {
                    options.UseSqlite(string.Format(connstring, dbname));
                    options.UseLazyLoadingProxies(false);
                }

            });
            services.AddDbContext<MainDbContext>(options =>
            {

                Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
                options.UseLazyLoadingProxies(false);


            });

            IConfigurationSection connString = Configuration.GetSection("ConnectionStrings");

            services.Configure<ConnectionString>(connString);

            services.AddScoped<CommanProc>();
            services.AddScoped<ReceiptProc>();
            services.AddScoped<PaymentProc>();
            services.AddScoped<MotorememoProc>();
            services.AddScoped<CashBankBookProc>();
            services.AddScoped<DayBookProc>();
            services.AddScoped<LedgerProc>();
            services.AddScoped<SubGroupListProc>();
            services.AddScoped<TrialBalanceProc>();
            services.AddScoped<TrialBalance1Proc>();
            services.AddScoped<BalanceSheetProc>();
            services.AddScoped<ProfitLossProc>();
            services.AddScoped<MotormemoRegProc>();

            services.AddControllers();


            services.AddControllersWithViews()
             .AddNewtonsoftJson(options =>
             {


                 options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                 options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                 options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                 options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                 //options.SerializerSettings.Culture = CultureInfo.GetCultureInfo("en-IN");//CultureInfo.InvariantCulture;  
             });





            // services.AddControllersWithViews()
            //.AddNewtonsoftJson(options =>
            //options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //services.AddControllersWithViews()
            // .AddJsonOptions(options =>
            // {
            //     options.JsonSerializerOptions.PropertyNamingPolicy = null;
            // });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
 
            app.Use((context, next) =>
            {
                if (context.Request.Headers["database"].ToString() != String.Empty)
                {
                    dbname = context.Request.Headers["database"].ToString();
                    Configuration["DataBaseName"]=dbname;
                }
                return next.Invoke();
            });

           


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                 
            }

            app.UseCors("MyPolicy");


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

}