
//using Microsoft.IdentityModel.Tokens;
//using Newtonsoft.Json.Serialization;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//// ====================================
//// 1️⃣ Add services
//// ====================================

//// CORS
//builder.Services.AddCors(options =>
//{
//	options.AddPolicy("AllowOrigin", policy =>
//		policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
//});

//// JSON Serializer
//builder.Services.AddControllersWithViews()
//	.AddNewtonsoftJson(option =>
//		option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
//	.AddNewtonsoftJson(option =>
//		option.SerializerSettings.ContractResolver = new DefaultContractResolver());

//// Swagger
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//// JWT Authentication
//builder.Services.AddAuthentication("Bearer")
//	.AddJwtBearer("Bearer", options =>
//	{
//		options.TokenValidationParameters = new TokenValidationParameters
//		{
//			ValidateIssuer = false,
//			ValidateAudience = false,
//			ValidateLifetime = true,
//			ValidateIssuerSigningKey = true,
//			IssuerSigningKey = new SymmetricSecurityKey(
//				Encoding.UTF8.GetBytes("super-secret-key-1234567890-abcdef")),
//			RoleClaimType = "role"    // tên claim chứa quyền trong JWT
//		};
//	});

//builder.Services.AddAuthorization(option =>
//{
//	option.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
//});

//builder.Services.AddControllers();

//var app = builder.Build();

//// ====================================
//// 2️⃣ Configure middleware
//// ====================================

//if (app.Environment.IsDevelopment())
//{
//	app.UseDeveloperExceptionPage(); // ✅ Show exception page in dev
//	app.UseSwagger();
//	app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseStaticFiles(); // nếu có dùng tệp tĩnh như ảnh, css

//app.UseRouting();

//// ✅ ĐÚNG THỨ TỰ: Authentication → Authorization → Cors
//app.UseAuthentication();
//app.UseAuthorization();
//app.UseCors("AllowOrigin");

//// ✅ Endpoint mapping
//app.UseEndpoints(endpoints =>
//{
//	endpoints.MapControllers();
//});

//app.Run();
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ====================================
// 1️⃣ CẤU HÌNH DỊCH VỤ (DI Container)
// ====================================

// 🔓 Cho phép các domain khác gọi API (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowOrigin", policy =>
		policy.WithOrigins("http://localhost:4200")         // Cho phép c domain
			  .AllowAnyHeader()         // Cho phép tất cả các header
			  .AllowAnyMethod()
			  );       // Cho phép tất cả các phương thức (GET, POST, ...)
});

// 🧾 Cấu hình JSON khi trả về từ API
builder.Services.AddControllersWithViews()
	.AddNewtonsoftJson(option =>
		option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)  // Bỏ qua vòng lặp tham chiếu (tránh lỗi khi có dữ liệu cha-con lồng nhau)
	.AddNewtonsoftJson(option =>
		option.SerializerSettings.ContractResolver = new DefaultContractResolver());  // Giữ nguyên định dạng tên thuộc tính (không đổi sang camelCase)

// 📘 Swagger (API Docs UI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔐 Cấu hình xác thực bằng JWT Bearer
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
				Encoding.UTF8.GetBytes("super-secret-key-1234567890-abcdef")),

			// ✅ Bắt buộc: ánh xạ đúng field chứa role trong JWT
			RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
		};
	});

// ✅ Tùy chọn: dùng nếu bạn áp dụng [Authorize(Policy = "Admin")]
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

// Đăng ký Controller
builder.Services.AddControllers();

// ====================================
// 2️⃣ CẤU HÌNH PIPELINE MIDDLEWARE
// ====================================

var app = builder.Build();
app.UseCors("AllowOrigin");
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage(); // ✅ Hiện chi tiết lỗi khi chạy môi trường phát triển
	app.UseSwagger();                // ✅ Dùng Swagger cho môi trường dev
	app.UseSwaggerUI();             // ✅ Giao diện UI của Swagger
}

app.UseHttpsRedirection();          // ✅ Tự động chuyển hướng HTTP → HTTPS

app.UseStaticFiles();               // ✅ Phục vụ các file tĩnh (ảnh, css, js) từ wwwroot/

app.UseRouting();                   // ✅ Định tuyến yêu cầu đến Controller

// ⚠️ Lưu ý: thứ tự rất quan trọng khi dùng JWT + CORS
app.UseAuthentication();            // ✅ Xác thực (phải đứng trước Authorization)
app.UseAuthorization();             // ✅ Phân quyền
app.UseCors("AllowOrigin");         // ✅ Cho phép CORS (cần đặt sau Authentication nếu token được gửi qua header)

// ✅ Kết nối route tới controller
app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();      // Map tất cả các controller có attribute [ApiController]
});

app.Run(); // ✅ Khởi chạy ứng dụng
