using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Thêm mới
// addCors
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

//Đoạn code trên cho phép frontend (giao diện web) từ bất kỳ đâu gọi API này mà không bị chặn, kể cả gọi từ domain khác.

//JSON Serializer

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(option =>
    option.SerializerSettings.ReferenceLoopHandling = Newtonsoft
    .Json.ReferenceLoopHandling.Ignore)
    .AddNewtonsoftJson(option =>
    option.SerializerSettings.ContractResolver = new DefaultContractResolver());



//Cấu hình này giúp:

//Gọi API với dữ liệu JSON mà không bị lỗi vòng lặp khi có quan hệ đối tượng phức tạp.

//Đảm bảo JSON trả về giữ nguyên tên biến đúng như trong C#.

//Cho phép dùng MVC trong ASP.NET Core.






builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




////Đăng kí dịch vụ xác thực của JWT 
builder.Services.AddAuthentication("Bearer")
	.AddJwtBearer("Bearer", options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = false,
			ValidateAudience = false,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes("super-secret-key-1234567890-abcdef")) // phải giống khi tạo token
		};
	});



var app = builder.Build();

///Add UseAuthentication: xác thực người dùng
///Add UseAuthorization:phân quyền truy cập
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
//Tránh lỗi CORS khi frontend (như Angular, React, Vue) gọi API từ một domain khác với backend.




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage(); // ✅ Thêm dòng này để hiển thị lỗi chi tiết
	app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
