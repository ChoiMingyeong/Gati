
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace GatiToolkit.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //// 1) 인증 스킴 등록: JWT Bearer
            //builder.Services
            //    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        // 토큰 검증 파라미터
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,          // iss 검사
            //            ValidateAudience = true,        // aud 검사
            //            ValidateIssuerSigningKey = true,// 서명 키 검사
            //            ValidateLifetime = true,        // exp/nbf 검사
            //            ClockSkew = TimeSpan.Zero,      // 만료 오차 허용(기본 5분 → 0으로 엄격화)

            //            ValidIssuer = "my-issuer",
            //            ValidAudience = "my-audience",
            //            IssuerSigningKey = new SymmetricSecurityKey(
            //                Encoding.UTF8.GetBytes("super_secret_256bit_key_here"))
            //        };

            //        // 필요 시: 쿼리스트링/쿠키에서 토큰 읽기 커스터마이즈
            //        // options.Events = new JwtBearerEvents {
            //        //   OnMessageReceived = ctx => {
            //        //       if (ctx.Request.Query.TryGetValue("access_token", out var t))
            //        //           ctx.Token = t;
            //        //       return Task.CompletedTask;
            //        //   }
            //        // };
            //    });

            //builder.Services.AddAuthorization();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddEndpointsApiExplorer();

            //builder.WebHost.ConfigureKestrel(options =>
            //{
            //    //options.ListenAnyIP(port);
            //});

#if DEBUG
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(p =>
            {
                p.AddDefaultPolicy(p =>
                {
                    p.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
#else
            builder.Services.AddCors(p =>
            {
                p.AddDefaultPolicy(p =>
                {
                    p.WithOrigins([$"http://localhost:{port}", $"https://localhost:{port}"])
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });
#endif

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                // https://localhost:7048/swagger
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
